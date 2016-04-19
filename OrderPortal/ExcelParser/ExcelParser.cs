using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Web.UI.Repositories.DomainModels;
using System.Data;
using System.Data.OleDb;
using DataTable = System.Data.DataTable;

namespace ExcelParser
{
    public class ExcelParser
    {
        private SpreadsheetDocument spreadsheetDoc;
        private readonly List<PurchaseOrderWithLineItems> purchaseOrders;
        private List<IssueAndError> issuesAndErrors;
        public List<IssueAndError> IssueAndErrors
        {
            get { return issuesAndErrors; }
        }
        static private readonly Collection<char> OrderHeaderHeadingColumn = new Collection<char>()
        {
            {'A'},
            {'C'},
        };

        private Dictionary<object, string> FieldCellReference = new Dictionary<object, string>();
        private const string BatchPoSheetName = "Batch PO";
        private const string BatchPoTitleDelimiter = "Microsoft Volume Licensing Order Sheet";
        private static readonly string[] AllowedPurchaseOrderTypes ={"NE", "BEC", "ZU"};


        public ExcelParser()
        {
            issuesAndErrors = new List<IssueAndError>();
            purchaseOrders = new List<PurchaseOrderWithLineItems>();
        }


        public SharedStringTablePart SharedStringTablePart
        {
            get
            {
                if (spreadsheetDoc != null && spreadsheetDoc.WorkbookPart != null)
                {
                    return spreadsheetDoc.WorkbookPart.SharedStringTablePart;
                }
                return null;
            }
        }

        public List<PurchaseOrderWithLineItems> Parse(Stream streamSpreadSheet)
        {
            spreadsheetDoc = SpreadsheetDocument.Open(streamSpreadSheet, false);
            var workbook = spreadsheetDoc.WorkbookPart;
            Sheet wbsheet = workbook.Workbook.Descendants<Sheet>().FirstOrDefault(t => t.Name == BatchPoSheetName);
            if (wbsheet == null)
            {
                issuesAndErrors.Add(new IssueAndError
                {
                    Category = Category.Error,
                    Message = "Failed to locate the PO sheet, please make sure the sheet name of PO is \"Batch PO\"",
                    Severity = Severity.Critical
                });
                return null;
            }
            var sheetData = ((WorksheetPart) workbook.GetPartById(wbsheet.Id)).Worksheet.Elements<SheetData>().First();
            string cellValue = string.Empty;
            PurchaseOrderWithLineItems po = null;
            OrderLineItem orderLineItem = null;
            ReadingStatus readingStatus = ReadingStatus.Start;
            bool lineItemUpdateFlag = false;
            foreach (var row in sheetData.Elements<Row>())
            {
                foreach (var cell in row.Elements<Cell>())
                {
                    cellValue = GetCellValue(cell);
                    if (cellValue == BatchPoTitleDelimiter)
                    {
                        break;
                    }
                    switch (readingStatus)
                    {
                        case ReadingStatus.ReadingPOHeader:
                            if (OrderHeaderHeadingColumn.Contains(cell.CellReference.Value[0]))
                            {
                                string fieldName = GetFieldName(cellValue);
                                object model = GetModel(cellValue, po);
                                if (model == null)
                                {
                                    // Can't get model, consider change the reading mode
                                    if (cell.CellReference.Value[0] == 'A' && cellValue == "Line Number")
                                    {
                                        readingStatus = ReadingStatus.ReadingLineItemHeading;
                                    }
                                    else
                                    {
                                        issuesAndErrors.Add(new IssueAndError
                                        {
                                            Category =Category.Error,
                                            Message = string.Format("The field {0} is not supported at cell {1}, PO information may be ignored, please make sure you are using the latest template", fieldName, cell.CellReference.Value),
                                            Severity = Severity.Medium
                                        });
                                    }
                                    continue;
                                }
                                var value = GetCellValue(cell.NextSibling<Cell>());
                                var modelProperty = model.GetType().GetProperty(fieldName);

                                if (modelProperty!=null &&( modelProperty.PropertyType == typeof (DateTime) ||
                                    modelProperty.PropertyType == typeof(DateTime?)))
                                {
                                    var date = new DateTime();
                                    if (DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out date))
                                    {
                                        modelProperty.SetValue(model, date);
                                    }
                                    else
                                    {
                                        issuesAndErrors.Add(new IssueAndError
                                        {
                                            Category = Category.Issue,
                                            Message = string.Format("A valid date is expected at cell {0}", cell.NextSibling<Cell>().CellReference.Value),
                                            Severity = Severity.Critical
                                        });
                                    }
                                }
                                else if (modelProperty != null && modelProperty.PropertyType == typeof(string))
                                {
                                    modelProperty.SetValue(model, value);
                                }
                            }
                            break;
                        case ReadingStatus.ReadingLineItemHeading:
                            if (cell.CellReference.Value[0] == 'A')
                            {
                                readingStatus = ReadingStatus.ReadingLineItemRow;
                                orderLineItem = new OrderLineItem();
                                if (UpdateLineItem(cell, ref orderLineItem))
                                {
                                    lineItemUpdateFlag = true;
                                }
                                
                            }
                            else
                            {
                                // Still in reading line item headings, verify if the heading match the map
                                string headingTitle;
                                if (!ColumnMapToLineItemField.TryGetValue(cell.CellReference.Value[0].ToString(),out headingTitle) || cellValue != headingTitle)
                                {
                                    issuesAndErrors.Add(new IssueAndError
                                    {
                                        Category = Category.Error,
                                        Message = string.Format("Unexpected line item heading title found at cell {0}, please ensure you are using the latest template",cell.CellReference.Value),
                                        Severity = Severity.Critical
                                    });
                                }

                            }
                            break;
                        case ReadingStatus.ReadingLineItemRow:
                            if (UpdateLineItem(cell, ref orderLineItem))
                            {
                                lineItemUpdateFlag = true;
                            }
                            break;
                    }
                }
                if (cellValue == BatchPoTitleDelimiter)
                {
                    if (po != null)
                    {
                        purchaseOrders.Add(po);
                    }
                    po = new PurchaseOrderWithLineItems();
                    readingStatus = ReadingStatus.ReadingPOHeader;
                }
                if (readingStatus == ReadingStatus.ReadingLineItemRow && po!=null)
                {
                    if (lineItemUpdateFlag)
                    {
                        po.LineItems.Add(orderLineItem);
                    }
                    else
                    {
                        issuesAndErrors.Add(new IssueAndError
                        {
                            Category = Category.Issue,
                            Message = string.Format("empty line item row has been ignored at {0}", row.RowIndex),
                            Severity = Severity.Low
                        });
                    }
                    lineItemUpdateFlag = false;
                    orderLineItem = new OrderLineItem();
                }
            }
            if (po != null)
            {
                purchaseOrders.Add(po);
            }
            purchaseOrders.ForEach(p => p.LineItems.ForEach(poli =>
            {
                if (poli.PurchaseUnitQuantity == "CT")
                {
                    poli.PurchaseUnitQuantity = "-1";
                }
            }));
            ValidateParsedOrders();
            return purchaseOrders;
        }

    
        public List<PurchaseOrderWithLineItems> ParseXls(string filePath)
        {
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\"";
            OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
            oleDbConnection.Open();
            OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * from [{0}$]",BatchPoSheetName), oleDbConnection);
            
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                dbDataAdapter.Fill(dt);
            }
            catch (OleDbException ex)
            {
                issuesAndErrors.Add(new IssueAndError
                {
                    Category = Category.Error,
                    Message = "Failed to locate the PO sheet, please make sure the sheet name of PO is \"Batch PO\"",
                    Severity = Severity.Critical
                });
                return null;
            }
            
            PurchaseOrderWithLineItems po = null;
            OrderLineItem orderLineItem = null;
            ReadingStatus readingStatus = ReadingStatus.Start;
            bool lineItemUpdateFlag = false;
            string cellValue = string.Empty;
            string headingName = string.Empty;
            int rowIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                rowIndex++;
                int columnIndex = 0;
                foreach (var cell in row.ItemArray)
                {
                    columnIndex++;

                    cellValue = cell is DBNull ? string.Empty : (string) cell;
                    if (cellValue == BatchPoTitleDelimiter)
                    {
                        break;
                    }
                    switch (readingStatus)
                    {
                        case ReadingStatus.ReadingPOHeader:
                            if (columnIndex%2 == 1)
                            {
                                // read the header name
                                headingName = cellValue;
                                if (cellValue == "Line Number")
                                {
                                    readingStatus = ReadingStatus.ReadingLineItemHeading;
                                }
                                else
                                {
                                }
                                continue;
                            }
                            else
                            {
                                // Read the content
                                string fieldName = GetFieldName(headingName);
                                object model = GetModel(headingName, po);

                                if (model != null)
                                {
                                    var modelProperty = model.GetType().GetProperty(fieldName);
                                    if (modelProperty != null && (modelProperty.PropertyType == typeof (DateTime) ||
                                                                  modelProperty.PropertyType == typeof (DateTime?)))
                                    {
                                        var date = new DateTime();
                                        if (DateTime.TryParseExact(cellValue, "yyyyMMdd", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            modelProperty.SetValue(model, date);
                                        }
                                        else
                                        {

                                            issuesAndErrors.Add(new IssueAndError
                                            {
                                                Category = Category.Issue,
                                                Message =
                                                    string.Format("A valid date is expected at cell {0}",
                                                        GetCellReferenceCode(rowIndex, columnIndex)),
                                                Severity = Severity.Low
                                            });
                                        }
                                    }
                                    else if (modelProperty != null && modelProperty.PropertyType == typeof (string))
                                    {
                                        modelProperty.SetValue(model, cellValue);
                                    }
                                }
                                else
                                {
                                    // can't find the model, assume there is a error
                                    if (!string.IsNullOrEmpty(headingName))
                                    {
                                        issuesAndErrors.Add(new IssueAndError
                                        {
                                            Category = Category.Error,
                                            Message =
                                                string.Format(
                                                    "The field {0} is not supported at cell {1}, PO information may be ignored, please make sure you are using the latest template",
                                                    headingName, GetCellReferenceCode(rowIndex, columnIndex)),
                                            Severity = Severity.Medium
                                        });
                                    }
                                }
                                // clear the heading name after reading the header content
                                headingName = string.Empty;
                            }
                            break;
                        case ReadingStatus.ReadingLineItemHeading:
                            headingName = string.Empty;
                            if (columnIndex == 1)
                            {
                                readingStatus = ReadingStatus.ReadingLineItemRow;
                                orderLineItem = new OrderLineItem();
                            }
                            else
                            {
                                // Still in reading line item headings, verify if the heading match the map
                                string headingTitle;
                                if (!ColumnMapToLineItemField.TryGetValue(((char)('A' + columnIndex - 1)).ToString(), out headingTitle) || cellValue != headingTitle)
                                {
                                    issuesAndErrors.Add(new IssueAndError
                                    {
                                        Category = Category.Error,
                                        Message = string.Format("Unexpected line item heading title found at cell {0}, please ensure you are using the latest template", GetCellReferenceCode(rowIndex, columnIndex)),
                                        Severity = Severity.Critical
                                    });
                                }
                            }
                            break;
                        case ReadingStatus.ReadingLineItemRow:
                            if (UpdateLineItemValue(cellValue, ((char)('A' + columnIndex - 1)).ToString(), ref orderLineItem))
                            {
                                lineItemUpdateFlag = true;
                            }
                            break;
                    }
                }
                if (cellValue == BatchPoTitleDelimiter)
                {
                    if (po != null)
                    {
                        purchaseOrders.Add(po);
                    }
                    po = new PurchaseOrderWithLineItems();
                    readingStatus = ReadingStatus.ReadingPOHeader;
                }
                if (readingStatus == ReadingStatus.ReadingLineItemRow && po != null)
                {
                    if (lineItemUpdateFlag)
                    {
                        po.LineItems.Add(orderLineItem);
                    }
                    else
                    {
                        issuesAndErrors.Add(new IssueAndError
                        {
                            Category = Category.Error,
                            Message = string.Format("empty line item row has been ignored at {0}", rowIndex),
                            Severity = Severity.Low
                        });
                    }
                    lineItemUpdateFlag = false;
                    orderLineItem = new OrderLineItem();
                }
            }
            if (po != null)
            {
                purchaseOrders.Add(po);
            }
            purchaseOrders.ForEach(p=>p.LineItems.ForEach(poli=>
            {
                if (poli.PurchaseUnitQuantity == "CT")
                {
                    poli.PurchaseUnitQuantity = "-1";
                }
            }));
            ValidateParsedOrders();
            return purchaseOrders;
            
        }


        private bool UpdateLineItem(Cell cell, ref OrderLineItem orderLineItem)
        {
            string cellValue = GetCellValue(cell);
            return UpdateLineItemValue(cellValue,cell.CellReference.Value[0].ToString(), ref orderLineItem);
        }

        private bool UpdateLineItemValue(string cellValue,string columnIndex, ref OrderLineItem orderLineItem)
        {
            //string cellValue = GetCellValue(cell);
            string fieldDisplayName;
            bool updateFieldFlag = false;
            if (ColumnMapToLineItemField.TryGetValue(columnIndex, out fieldDisplayName))
            {
                string fieldName;
                if (LineItemFieldNameMapToModelProperty.TryGetValue(fieldDisplayName, out fieldName))
                {
                    var lineItemProperty = orderLineItem.GetType().GetProperty(fieldName);
                    if (lineItemProperty != null && lineItemProperty.PropertyType == typeof (int))
                    {
                        int iCellValue;
                        if (int.TryParse(cellValue, out iCellValue))
                        {
                            lineItemProperty.SetValue(orderLineItem, iCellValue);
                            updateFieldFlag = true;
                        }
                    }
                    else if (lineItemProperty != null && lineItemProperty.PropertyType == typeof (string))
                    {
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            lineItemProperty.SetValue(orderLineItem, cellValue);
                            updateFieldFlag = true;
                        }
                    }
                }
            }


            return updateFieldFlag;
        }

        private void ValidateParsedOrders()
        {
            foreach (var orderWithLineItems in purchaseOrders)
            {
                // Validate the Order header
                var orderHeader = orderWithLineItems.OrderHeader;
                if (string.IsNullOrEmpty(orderHeader.AgreementNumber))
                {
                    AddIssueOrErrorInfo("Agreement Number is missing", Severity.Critical, Category.Error, null);
                }
                if (orderHeader.UsageDate == null || orderHeader.UsageDate == DateTime.MinValue)
                {
                    AddIssueOrErrorInfo("Invalid usage date", Severity.Critical, Category.Error, null);
                }
                if (string.IsNullOrEmpty(orderHeader.PurchaseOrderNumber))
                {
                    AddIssueOrErrorInfo("There is empty purchase order number", Severity.Critical, Category.Error, null);
                }
                if (!AllowedPurchaseOrderTypes.Contains(orderHeader.PurchaseOrderTypeCode))
                {
                    AddIssueOrErrorInfo("There is empty purchase order type code", Severity.Critical, Category.Error, null);
                }
                foreach (var lineItem in orderWithLineItems.LineItems)
                {
                    // Validate the line items
                    if (string.IsNullOrEmpty(lineItem.PartNumber))
                    {
                            AddIssueOrErrorInfo("There is empty part number in the work sheet", Severity.Critical, Category.Error, null);
                    }
                    if (!(lineItem.QuantityOrdered > 0 && lineItem.QuantityOrdered < int.MaxValue))
                    {
                        AddIssueOrErrorInfo("There is empty part number in the work sheet", Severity.Critical, Category.Error, null);
                        
                    }
                    if (string.IsNullOrEmpty(lineItem.BillingOption))
                    {
                        AddIssueOrErrorInfo("There is empty billing option in the work sheet", Severity.Critical, Category.Error, null);
                    }
                    //if (string.IsNullOrEmpty(lineItem.ProgramOfferingCode))
                    //{
                    //    AddIssueOrErrorInfo("There is empty part number in the work sheet", Severity.Critical, Category.Error, null);
                    //}
                    if (string.IsNullOrEmpty(lineItem.UsageCountryCode))
                    {
                        AddIssueOrErrorInfo("There is empty usage country code in the work sheet", Severity.Critical, Category.Error, null);
                    }
                }
            }
        }

        private void AddIssueOrErrorInfo(string msg, Severity severity, Category category, string cellReference)
        {
            issuesAndErrors.Add(new IssueAndError
            {
                Message = string.Format(msg, cellReference),
                Category = category,
                Severity = severity
            });
        }

        private static string GetFieldName(string cellValue)
        {
            string fieldName;
            return FieldNameMapToPropertyDictionary.TryGetValue(cellValue, out fieldName) ? fieldName : null;
        }

        private object GetModel(string cellValue, PurchaseOrderWithLineItems po=null)
        {
            if (po == null) return null;
            string modelName;
            if (FieldNameMapToModelDictionary.TryGetValue(cellValue, out modelName))
            {
                switch (modelName)
                {
                    case "Shipment":
                        return po.Shipment;
                    case "OrderHeader":
                        return po.OrderHeader;
                    default:
                        return null;
                }
            }
            return null;
        }

        #region function list dictionary to setup the model field value

        public class functions<model>
        {
            public Func<string, object> GetValueMethod;
            public Action<model, object> SetValueMethod;
        }

        public IDictionary<string, functions<OrderHeader>> orderHeaderConfig = new Dictionary
            <string, functions<OrderHeader>>()
        {
            {
                "PONumber",
                new functions<OrderHeader>()
                {
                    SetValueMethod = (o, v) => { o.PurchaseOrderNumber = (string) v; },
                    GetValueMethod = (input) => (string)input
                }
            },
            {
                "POType",
                new functions<OrderHeader>()
                {
                    SetValueMethod = (o, v) => { o.PurchaseOrderTypeCode = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "UsageDate",
                new functions<OrderHeader>()
                {
                    SetValueMethod = (o, v) => { o.UsageDate = (DateTime?) v; },
                    GetValueMethod = (input) =>
                    {
                        DateTime date;
                        return DateTime.TryParseExact(input, "yyyyMMdd", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out date)
                            ? date
                            : (DateTime?)null;
                    }
                }
            },
            {
                "AgreementNumber",
                new functions<OrderHeader>()
                {
                    SetValueMethod = (o, v) => { o.AgreementNumber = (string) v; },
                    GetValueMethod = (input) => input
                }
            }
        };

        public IDictionary<string, functions<Shipment>> shipmentConfig = new Dictionary<string, functions<Shipment>>
        {
            {
                "ShipToOrganizationName",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.OrganizationName = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToContactName",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.ContactFirstName = (string) v; }, // TODO: firstname+lastname
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToAddress1",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.AddressLine1 = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToContactPhoneNumber",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.ContactPhoneNumber = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToAddress2",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.AddressLine2 = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToContactFaxNumber",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.ContactFaxNumber = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToAddress3",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.AddressLine3 = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToContactEmailAddress",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.ContactEmailAddress = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToAddress4",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.AddressLine4 = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToCorrespondenceLanguage",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.CorrespondenceLanguageCode = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToCity",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.City = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "CarrierCode",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { }, //TODO: Shipment model lack CarrierCode field
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToStateProvince",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.StateProvince = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "CarrierAccountNumber",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => {  }, // TODO: add the field
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToPostalCode",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.PostalCode = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "Reference",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { }, // TODO: add reference
                    GetValueMethod = (input) => input
                }
            },
            {
                "ShipToCountryCode",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.CountryCode = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "PublicCustomerNumber",
                new functions<Shipment>()
                {
                    SetValueMethod = (o, v) => { o.ShipToPartnerNumber = (string) v; }, //TODO: verify the field
                    GetValueMethod = (input) => input
                }
            },
        };

        public IDictionary<string, functions<OrderLineItem>> lineItemConfig = new Dictionary
            <string, functions<OrderLineItem>>
        {
            {
                "LineNo",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { }, // Do nothing
                    GetValueMethod = (input) => input
                }
            },
            {
                "MSPartNo",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.PartNumber = (string) v; }, 
                    GetValueMethod = (input) => input
                }
            },
            {
                "Qty",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.OrderQuantity = (int) v; }, 
                    GetValueMethod = (input) =>
                    {
                        int result;
                        return int.TryParse(input,out result)?result:0;
                    }
                }
            },
            {
                "BillingOption",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.BillingOption = (string) v; }, 
                    GetValueMethod = (input) => input
                }
            },
            {
                "ProgramOffering",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.ProgramOfferingCode = (string) v; }, 
                    GetValueMethod = (input) => input
                }
            },
            {
                "UsageCountry",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.UsageCountryCode = (string) v; }, 
                    GetValueMethod = (input) => input
                }
            },
            {
                "CustomerRefNumber",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.CustomerReferenceNumber = (string) v; }, 
                    GetValueMethod = (input) => input
                }
            },
            {
                "SpecialDealsNumber",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.SpecialDealNumber = (string) v; }, 
                    GetValueMethod = (input) => input
                }
            },
            {
                "UnitType",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.PurchaseUnitTypeCode = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "UnitQuantity",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => { o.PurchaseUnitQuantity = (string) v; },
                    GetValueMethod = (input) => input
                }
            },
            {
                "ReferenceID",
                new functions<OrderLineItem>()
                {
                    SetValueMethod = (o, v) => {  }, // Do nothing
                    GetValueMethod = (input) => input
                }
            },
        };

        #endregion

        public string GetCellReferenceCode(int rowIndex, int columnIndex)
        {
            // Transfer column index into letter
            if (columnIndex > 26 || columnIndex < 1) return "ERROR";
            string columnCode = ((char) (columnIndex + 'A' - 1)).ToString();
            return columnCode + rowIndex.ToString();
        }

        public string GetCellValue(Cell cell)
        {
            string cellValue = null;
            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {

                    case CellValues.SharedString:
                        cellValue =
                            SharedStringTablePart.SharedStringTable.ChildElements[int.Parse(cell.InnerText)]
                                .InnerText;
                        break;
                    case CellValues.Boolean:
                    case CellValues.Date:
                    case CellValues.Number:
                    case CellValues.String:
                    case CellValues.InlineString:
                        cellValue = cell.CellValue.InnerText;
                        break;
                    case CellValues.Error:
                        issuesAndErrors.Add(new IssueAndError
                        {
                            Category = Category.Error,
                            Message = string.Format("Error Happened when Reading Cell {0}", cell.CellReference.Value)
                        });
                        break;
                    default:
                        break;
                }
            }
            else
                cellValue = null;
            return cellValue;
        }


        private static readonly Dictionary<string, string> ColumnMapToLineItemField = new Dictionary<string, string>()
        {
            {"A", "Line Number"},
            {"B", "MS Part Number"},
            {"C", "Order Quantity"},
            {"D", "Billing Option"},
            {"E", "Offering Type"},
            {"F", "Usage Country"},
            {"G", "Customer Ref Number"},
            {"H", "Special Deals Number"},
            {"I", "Unit Type"},
            {"J", "Unit Quantity"},
            {"K", "Reference ID"}
        };

        private static readonly Dictionary<string, string> LineItemFieldNameMapToModelProperty = new Dictionary
            <string, string>()
        {
           // {"Line Number",""},
            {"MS Part Number", "PartNumber"},
            {"Order Quantity", "QuantityOrdered"},
            {"Billing Option","BillingOption"},
            {"Offering Type", "ProgramOfferingCode"},
            {"Usage Country","UsageCountryCode"},
            {"Customer Ref Number","CustomerReferenceNumber"},
            {"Special Deals Number","SpecialDealNumber"},
            {"Unit Type","PurchaseUnitType"},
            {"Unit Quantity","PurchaseUnitQuantity"},
            //{"Reference ID"}
        };

        private static readonly Dictionary<string, string> FieldNameMapToModelDictionary = new Dictionary<string, string>()
        {
            {"PO Number", "OrderHeader"},
            {"Usage Date (YYYYMMDD)", "OrderHeader"},
            {"PO Type", "OrderHeader"},
            {"Agreement Number", "OrderHeader"},
            {"Ship-To Organization Name", "Shipment"},
            {"Ship-To Address Line 1", "Shipment"},
            {"Ship-To Address Line 2", "Shipment"},
            {"Ship-To Address Line 3", "Shipment"},
            {"Ship-To Address Line 4", "Shipment"},
            {"Ship-To City", "Shipment"},
            {"Ship-To State Province", "Shipment"},
            {"Ship-To Postal Code", "Shipment"},
            {"Ship-To Country Code", "Shipment"},
            {"Ship-To Contact Name", "Shipment"},
            {"Ship-To Contact Phone Number", "Shipment"},
            {"Ship-To Contact Fax Number", "Shipment"},
            {"Ship-To Contact Email Address", "Shipment"},
            {"Ship-To Correspondence Language", "Shipment"},
            {"Carrier Code", "Shipment"},
            {"Carrier Account Number", "Shipment"},
            {"Reference", "Shipment"},
            {"Public Customer Number", "OrderHeader"}
        };

        private static readonly Dictionary<string, string> FieldNameMapToPropertyDictionary = new Dictionary<string, string>()
        {
            {"PO Number", "PurchaseOrderNumber"},
            {"Usage Date (YYYYMMDD)", "UsageDate"},
            {"PO Type", "PurchaseOrderTypeCode"},
            {"Agreement Number", "AgreementNumber"},
            {"Ship-To Organization Name", "OrganizationName"},
            {"Ship-To Address Line 1", "AddressLine1"},
            {"Ship-To Address Line 2", "AddressLine2"},
            {"Ship-To Address Line 3", "AddressLine3"},
            {"Ship-To Address Line 4", "AddressLine4"},
            {"Ship-To City", "City"},
            {"Ship-To State Province", "StateProvince"},
            {"Ship-To Postal Code", "PostalCode"},
            {"Ship-To Country Code", "CountryCode"},
            {"Ship-To Contact Name", "ContactFirstName"}, // TODO: 
            {"Ship-To Contact Phone Number", "ContactPhoneNumber"},
            {"Ship-To Contact Fax Number", "ContactFaxNumber"},
            {"Ship-To Contact Email Address", "ContactEmailAddress"},
            {"Ship-To Correspondence Language", "CorrespondenceLanguageCode"},
            {"Carrier Code", "CarrierCode"},
            {"Carrier Account Number", "CarrierAccountNumber"},
            {"Reference", "Reference"},
            {"Public Customer Number", "EndCustomerNumber"}
        };

        private static readonly Dictionary<string, LineItemFieldAttribute> LineItemFieldAttributes = new Dictionary<string, LineItemFieldAttribute>
                {
           // {"Line Number",""},
            {"MS Part Number", new LineItemFieldAttribute{FieldName =  "PartNumber", IsMandatory = true}},
            {"Order Quantity", new LineItemFieldAttribute{FieldName = "QuantityOrdered", IsMandatory = true}},
            {"Billing Option", new LineItemFieldAttribute{FieldName = "BillingOption", IsMandatory = true}},
            {"Offering Type", new LineItemFieldAttribute{FieldName = "ProgramOfferingCode", IsMandatory = true}},
            {"Usage Country",new LineItemFieldAttribute{FieldName = "UsageCountryCode",IsMandatory = true}},
            {"Customer Ref Number",new LineItemFieldAttribute{FieldName = "CustomerReferenceNumber",IsMandatory = false}},
            {"Special Deals Number",new LineItemFieldAttribute{FieldName = "SpecialDealNumber", IsMandatory = false}},
            {"Unit Type",new LineItemFieldAttribute{FieldName = "PurchaseUnitType", IsMandatory = false}},
            {"Unit Quantity",new LineItemFieldAttribute{FieldName = "PurchaseUnitQuantity",IsMandatory = false}},
            //{"Reference ID"}
        };

        private class LineItemFieldAttribute
        {
            public bool IsMandatory { get; set; }
            public string FieldName { get; set; }
        }

        enum ReadingStatus
        {
            Start,
            End,
            ReadingPOHeader,
            ReadingLineItemHeading,
            ReadingLineItemRow
        }
    }
}
