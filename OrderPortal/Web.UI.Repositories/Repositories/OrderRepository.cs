using System.Configuration;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PlainElastic.Net.Utils;
using Web.UI.Common.Extensions;
using Web.UI.Repositories.Data;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.DraftOrderServiceProxy;
using Web.UI.ServiceGateway.OrderServiceProxy;
using Web.UI.Repositories.Bridges;
using Web.UI.Common.Extensions;
using Web.Localization;

namespace Web.UI.Repositories
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {

        public OrderRepository()
        {
            OrderBridge = new OrderServiceBridge();
        }
        public IOrderBridge OrderBridge { get; set; }

        public async Task<SearchResult<OpportunityItem>> GetNonResStepUpOpportunties(OpportunityRequest orderParam)
        {
            return await GetOpportunityItem(orderParam, OpportunityType.StepUp);
        }

        public async Task<SearchResult<OpportunityItem>> GetRenewalOpportunties(OpportunityRequest orderParam)
        {
            return await GetOpportunityItem(orderParam, OpportunityType.Renewal);
        }
        // TODO: Boil down to two with Generics if possible

        public async Task<SearchResult<OpportunityItem>> GetTrueUpOpportunities(OpportunityRequest orderParam)
        {
            return await GetOpportunityItem(orderParam, OpportunityType.EligibleProduct);
        }


        public async Task<IEnumerable<DomainItem>> GetOrderHeaderAttributes(int agreementId)
        {
            List<DomainItem> OutputRecords = new List<DomainItem>() { };

            var client = new OrderServiceClient();
            IEnumerable<DomainItemResponse> response;
            response = client.GetOrderHeaderAttributes(agreementId).ToArray();
            List<DomainItemResponse> resp = response.Cast<DomainItemResponse>().ToList();
            foreach (var item in resp.ToList())
            {
                OutputRecords.Add(Mapper.Map<DomainItemResponse, DomainItem>(item));
            }
            return OutputRecords;

        }

        public async Task<PriceAtNewLevel> GetPriceEstimate(OrderLineItem lineItem, Agreement agreementheader, string poType)
        {
            var vlHeader = new VLHeader
            {
                AgreementNumber = agreementheader.AgreementNumber,
                EndCustomerNumber = agreementheader.EndCustomerNumber,
                UsagePeriodDate = lineItem.POLIUsageDateTime,
                RequestDate = DateTime.Now,// TODO
                PurchaseOrderTypeCode = poType,
                HeaderGuid = Guid.NewGuid(),
                PurchaseOrderDetailsType = PODetailsType.AdditionalProducts
            };

            lineItem.PurchaseUnitTypeCode = "MT";//this is the only choice this field have when OLS, this field doens't apply for Non-OLS

            //var itemDetails = new VLItemInfo
            //{
            //    Number = lineItem.PartNumber,
            //    Name = lineItem.ItemName,
            //    ProductFamilyCode = lineItem.ProductFamilyCode,
            //    ProductTypeCode = lineItem.ProductTypeCode,

            //};

            //var vlLine = new VLLineItem
            //{
            //    Id = 1,
            //    BillingOptionCode = "AE",
            //    UsageCountryCode = "US",
            //    POLIUsageDateTime = DateTime.Parse("2014-10-01 00:00:00"),
            //    ItemDetails = itemDetails,
            //    AvailableQuantity = 1,
            //    OrderQuantity = 1,
            //    CoverageStartDate = DateTime.Parse("2014-10-01 00:00:00"),
            //    CoverageEndDate = DateTime.Parse("2016-10-01 00:00:00"),
            //    UnitOfMeasure = "MON",
            //    PurchaseUnitQuantity = 12,
            //    PurchaseUnitTypeCode = "MT",// TODO
            //    ProgramOfferingCode = "ACP",
            //    LineItemNumber = Guid.NewGuid().ToString(),
            //    LineItemGuid = Guid.NewGuid()

            //};

            var priceLevel = new PriceAtNewLevel();
            var client = new OrderServiceClient();
            // var lin = Mapper.Map<OrderLineItem, VLLineItem>(lineItem);
            var request = new GetVLOrderEstimatesRequest
            {
                RequestId = Guid.NewGuid().ToString(),
                //ApplicationId=ConfigData.ApplicationId,
                ApplicationId = "POET",
                CandidateVlOrders = new []{
                    new VLOrder
                        {
                            Header = vlHeader,
                            LineItems =new [] {
                            Mapper.Map<OrderLineItem, VLLineItem>(lineItem)
                            }
                        }
                    }
            };
            var result = await client.GetOrderEstimatesAsync(request);
            var orderResponse = result as GetVLOrderEstimatesResponse;
            if (orderResponse != null &&
                      orderResponse.CandidateVLOrders != null &&
                      orderResponse.CandidateVLOrders[0].LineItems.Any())
            {
                priceLevel.SystemPrice = orderResponse.CandidateVLOrders[0].LineItems[0].SystemPrice ?? 0;
                priceLevel.ListPrice = orderResponse.CandidateVLOrders[0].LineItems[0].ListPrice ?? 0;
                priceLevel.PurchaseUnitQuantity = orderResponse.CandidateVLOrders[0].LineItems[0].PurchaseUnitQuantity;

            }
            return priceLevel;

        }

        public async Task<IEnumerable<DomainItem>> GetLineItemAttributes(LineItemAttributeRequest lineItemRequest)
        {
            List<DomainItem> OutputRecords = new List<DomainItem>() { };
            var client = new OrderServiceClient();
            IEnumerable<DomainItemResponse> response;
            response = await Task.Factory.StartNew(() => client.GetLineItemAttributes(lineItemRequest).ToArray());
            List<DomainItemResponse> resp = response.Cast<DomainItemResponse>().ToList();
            foreach (var item in resp.ToList())
            {
                OutputRecords.Add(Mapper.Map<DomainItemResponse, DomainItem>(item));
            }
            return OutputRecords;

        }

        public async Task<IEnumerable<CreateVLOrderResponse>> CreateOrder(OrderHeader orderHeader, List<OrderLineItem> lineItems, Shipment shipment, string userName)
        {
            var audit = new Audit
            {
                CreatedBy = userName,
                ModifiedBy = userName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            var client = new OrderServiceClient();
            var createVLOrderRequest = BuildCreateVlOrderRequest(orderHeader, lineItems, audit);
            var response = await client.CreateOrderAsync(new[] { createVLOrderRequest });
            if (response != null)
            {
                return response.Cast<CreateVLOrderResponse>().ToList();
            }
            return null;
        }

        public CreateVLOrderRequest BuildCreateVlOrderRequest(OrderHeader orderHeader, List<OrderLineItem> lineItems, Audit audit)
        {

            var vlHeader = BuildVLHeader(orderHeader, audit);
            var vlLineItems = new List<VLLineItem>();
            lineItems.ForEach(l => vlLineItems.Add(BuildVLLineItem(l, orderHeader.ProgramCode)));
            var order = new VLOrder()
            {
                Header = vlHeader,
                IsSubmitAsync =
                    ConfigurationManager.AppSettings["EnableAsyncOrderSubmit"].Equals("true",
                        StringComparison.OrdinalIgnoreCase),
                LineItems = vlLineItems.ToArray()
            };
            var createVlOrderRequest = new CreateVLOrderRequest()
            {
                Order = order,
                UserName = audit.CreatedBy,
                ApplicationId = "POET",
                RequestId = Guid.NewGuid().ToString(),
            };

            return createVlOrderRequest;
        }

        private VLHeader BuildVLHeader(OrderHeader order, Audit audit)
        {
            var vlHeader = Mapper.Map<OrderHeader, VLHeader>(order);
            vlHeader.TransactionSetPurposeCode = "00";
            vlHeader.CreatedFromInvalidPurchaseOrder = false; // TODO: when implementing the invalid process, this should be updated
            vlHeader.Audit = audit;
            vlHeader.PurchaseOrderTypeCode = string.IsNullOrEmpty(order.PurchaseOrderTypeCode)
                ? "NE"
                : order.PurchaseOrderTypeCode;
            vlHeader.PurchaseOrderStatusCode = "HRW";
            vlHeader.EndCustomerPublicCustomerNumber = order.EndCustomerNumber;
            vlHeader.SourceCode = string.IsNullOrEmpty(order.SourceCode) ? "MAN" : order.SourceCode;
            return vlHeader;
        }

        private VLLineItem BuildVLLineItem(OrderLineItem orderLineItem, string programCode)
        {
            var vlLineItem = Mapper.Map<OrderLineItem, VLLineItem>(orderLineItem);
            //vlLineItem.BillingOptionCode = "AE";
            //vlLineItem.PurchaseUnitQuantity = 12; // Will be passed from orderLineItem
            //vlLineItem.PurchaseUnitTypeCode = "MT";
            //vlLineItem.UnitOfMeasure = "EA";
            //vlLineItem.IsOlsItem = true;

            vlLineItem.PurchaseUnitCode = "1M";
            if (vlLineItem.OrderQuantity == 0 && orderLineItem.QuantityOrdered != 0)
            {
                vlLineItem.OrderQuantity = orderLineItem.QuantityOrdered;
            }
            
            
            var ret = OAPCoverageDateCalculation(programCode, DateTime.Now, null, null, orderLineItem.CoverageEndDate, string.IsNullOrEmpty(orderLineItem.PurchaseUnitQuantity) ? "0" : orderLineItem.PurchaseUnitQuantity, orderLineItem.BillingOption);
            vlLineItem.CoverageStartDate = ret.Key;
            vlLineItem.CoverageEndDate = ret.Value;
            return vlLineItem;
        }

        public async Task<long> LockDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            var dosClient = new DraftOrderServiceClient();
            var req = new LockDraftOrderPortalRequest()
            {
                Id = draftOrderId,
                UserName = userName,
                MaxLockMinutes = maxLockMinutes,
            };
            var resp = await dosClient.LockDraftOrderPortalAsync(req);
            return resp.IssueId;
        }

        public async Task<long> UnlockDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            var dosClient = new DraftOrderServiceClient();
            var req = new UnlockDraftOrderPortalRequest()
            {
                Id = draftOrderId,
                UserName = userName,
                MaxLockMinutes = maxLockMinutes,
            };
            var resp = await dosClient.UnlockDraftOrderPortalAsync(req);
            return resp.IssueId;
        }

        public async Task<long> DeleteDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            var dosClient = new DraftOrderServiceClient();
            var req = new DeleteDraftOrderPortalRequest()
            {
                Id = draftOrderId,
                UserName = userName,
                MaxLockMinutes = maxLockMinutes,
            };
            var resp = await dosClient.DeleteDraftOrderPortalAsync(req);
            return resp.IssueId;
        }

        public async Task<List<string>> ValidateDraftOrder(OrderHeader order, List<OrderLineItem> orderLineitems, string userName)
        {
            List<string> errorMsgs = new List<string>();

            // do simple validation           
            if (string.IsNullOrEmpty(order.AgreementNumber) || order.AgreementNumber.Length > 12)
            {
                errorMsgs.Add("Error_AgreementNumberEmptyOrOverMaxLength".Localize());//"AgreementNumber cannot be empty or its max length is 12"
            }
            if (string.IsNullOrEmpty(order.PurchaseOrderTypeCode))
            {
                errorMsgs.Add("Error_PurchaseOrderTypeEmpty".Localize());//"Purchase Order Type cannot be empty");
            }
            if (string.IsNullOrEmpty(order.PurchaseOrderNumber) || order.PurchaseOrderNumber.Length > 30)
            {
                errorMsgs.Add("Error_PurchaseOrderNumberEmptyOrOverMaxLength".Localize());//PurchaseOrderNumber cannot be empty or its max length is 30");
            }
            if (string.IsNullOrEmpty(order.DirectCustomerNumber))
            {
                errorMsgs.Add("Error_DirectPartnerNumberEmpty".Localize()); //Direct Partner Number cannot be empty");
            }

            if (orderLineitems == null || orderLineitems.Count() == 0)
            {
                if (string.IsNullOrEmpty(order.PurchaseOrderTypeCode) || order.PurchaseOrderTypeCode.Trim().ToLower() != "zu")
                {
                    errorMsgs.Add("Line items cannot be empty");
                }
            }
            else
            {
                if (!order.UsageDate.HasValue)
                {
                    errorMsgs.Add("UsageDate cannot be empty");
                }

                for (int i = 0; i < orderLineitems.Count(); i++)
                {
                    if (string.IsNullOrEmpty(orderLineitems[i].PartNumber) || orderLineitems[i].PartNumber.Length > 16)
                    {
                        errorMsgs.Add("NO".Localize() + (i + 1) + "Error_PartNumberEmptyOrOverMaxLength");// item's PartNumber cannot be empty and its max length is 16");
                    }
                    if (!(orderLineitems[i].QuantityOrdered < 10000000 && orderLineitems[i].QuantityOrdered > 0))
                    {
                        errorMsgs.Add("NO".Localize() + (i + 1) + "Error_OrderQuantityNotInRange");// item's OrderQuantity must be > 0 and < 10000000");
                    }
                    //if (string.IsNullOrEmpty(orderLineitems[i].ProgramOfferingCode))
                    //{
                    //    errorMsgs.Add("No." + (i + 1) + " item's Program Offering cannot be empty");
                    //}
                    if (!orderLineitems[i].POLIUsageDateTime.HasValue)
                    {
                        errorMsgs.Add("NO".Localize() + (i + 1) + "Error_UsageDateEmpty");// item's Usage Date cannot be empty");
                    }
                    if (string.IsNullOrEmpty(orderLineitems[i].UsageCountryCode))
                    {
                        errorMsgs.Add("NO".Localize() + (i + 1) + "Error_UsageCountryEmpty");// item's Usage Country cannot be empty");
                    }
                    if (orderLineitems[i].IsOlsItem && string.IsNullOrEmpty(orderLineitems[i].PurchaseUnitQuantity))
                    {
                        errorMsgs.Add("NO".Localize() + (i + 1) + "Error_UnitQuantityEmpty");//" item's Unit Quantity cannot be empty");
                    }
                    //TODO: BillingOption's validation relies on other reference data. Can't be dealt with here simply
                    //if (string.IsNullOrEmpty(orderLineitems[i].BillingOption))
                    //{
                    //    errorMsgs.Add("No." + (i + 1) + " item's Billing Option cannot be empty");
                    //}
                }
            }

            // do complex validation
            var client = new DraftOrderServiceClient();
            ValidateDraftOrderPortalRequest request = BuildValidateDraftOrderRequest(order, orderLineitems, userName);
            var ret = await client.ValidateDraftOrderPortalAsync(request);
            if (ret.IssueId != 0)
            {
                var errors = ret.ErrorMsgs.Split(';');
                foreach(var error in errors)
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        errorMsgs.Add(error);
                    }
                }
            }

            return errorMsgs;
        }

        private ValidateDraftOrderPortalRequest BuildValidateDraftOrderRequest(OrderHeader order, List<OrderLineItem> orderLineitems, string userName)
        {
            var draftOrderAttrs = Mapper.Map<DraftOrderAttrs>(order);
            var draftOrderLineItems = orderLineitems.EmptyIfNull().Select(Mapper.Map<DraftOrderLineItem>).ToArray();
            return new ValidateDraftOrderPortalRequest
            {
                DraftOrderAttributes = draftOrderAttrs,
                DraftOrderLineItems = draftOrderLineItems,
                SourceSystem = order.SourceSystem,
                UserName = userName,
                AssignedTo = order.AssignedTo,
                LockedFlag = order.LockedFlag,
                LockedBy = order.LockedBy,
            };
        }

        public async Task<long> SaveDraftOrder(OrderHeader order, List<OrderLineItem> orderLineitems, string userName, Guid correlationId)
        {
            var client = new DraftOrderServiceClient();
            SaveDraftOrderPortalRequest request = BuildSaveDraftOrderRequest(order, orderLineitems, userName, correlationId);
            var ret = await client.SaveDraftOrderPortalAsync(request);
            return ret.Id;
        }
        private SaveDraftOrderPortalRequest BuildSaveDraftOrderRequest(OrderHeader order, List<OrderLineItem> orderLineitems, string userName, Guid correlationId)
        {
            var draftOrderAttrs = Mapper.Map<DraftOrderAttrs>(order);
            var draftOrderLineItems = orderLineitems.EmptyIfNull().Select(Mapper.Map<DraftOrderLineItem>).ToArray();
            return new SaveDraftOrderPortalRequest
            {
                DraftOrderAttributes = draftOrderAttrs,
                DraftOrderLineItems = draftOrderLineItems,
                SourceSystem = order.SourceSystem,
                UserName = userName,
                AssignedTo = order.AssignedTo,
                LockedFlag = order.LockedFlag,
                LockedBy = order.LockedBy,
                CorrelationId = correlationId,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="draftOrderId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>

        public async Task<IEnumerable<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>> GetDraftOrder(long? draftOrderId, string pcnFilter, string userName)
        {
            var client = new DraftOrderServiceClient();
            var request = new GetDraftOrderPortalRequest { Id = draftOrderId, PCNFilter = pcnFilter.IsNullOrEmpty() ? null : pcnFilter, UserName = userName };
            var ret = await client.GetDraftOrderPortalAsync(request);
            var result = from DraftOrderAttrs draftOrderAttrs in ret.DraftOrderAttrs.EmptyIfNull()
                         join DraftOrderLineItem draftOrderLineItem in ret.DraftOrderLineItem.EmptyIfNull()
                         on 1 equals 1 into lg
                         select new KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>
                         (Mapper.Map<OrderHeader>(draftOrderAttrs), lg.Select(Mapper.Map<OrderLineItem>));

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderParam"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<SearchResult<OpportunityItem>> GetOpportunityItem(OpportunityRequest orderParam, OpportunityType type)
        {
            var output = new SearchResult<OpportunityItem>
            {
                Results = new List<OpportunityItem>()
            };

            if (orderParam.AgreementNumber.Any())
            {
                var client = new OrderServiceClient();

                var response = client.GetOpportunities(BuildOpportunityRequest(orderParam, type).ToArray());
                foreach (var oppCont in response)
                {
                    foreach (var opp in oppCont.OpportunityContainers)
                {
                        var objOpportunities = new OpportunityItem
                    {
                            SourceLineItems = new List<OpportunityLineItem>(),
                            TargetLineItems = new List<OpportunityLineItem>()
                        };
                        foreach (var op in opp.Opportunities)
                        {
                            foreach (var li in op.TargetLineItems)
                            {
                                var opLineItem = Mapper.Map<VLLineItem, OpportunityLineItem>(li);
                                if (op.SourceLineItems.Length > 0)
                                {
                                    opLineItem.ParentPartNumber = op.SourceLineItems[0].ItemDetails.Number;
                                    opLineItem.ParentItemName = op.SourceLineItems[0].ItemDetails.Name;
                                    opLineItem.ParentItemId = op.SourceLineItems[0].ItemDetails.Id;
                                }
                                objOpportunities.TargetLineItems.Add(opLineItem);
                                output.TotalCount = li.TotalCount;
                                }
                    }

                        output.Results.Add(objOpportunities);
                }
            }
            }
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderParam"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private Collection<GetOpportunityRequest> BuildOpportunityRequest(OpportunityRequest orderParam, OpportunityType type)
        {
            Collection<GetOpportunityRequest> request = new Collection<GetOpportunityRequest> { new GetOpportunityRequest() {
                    ApplicationId = ConfigData.ApplicationId,
                    POAgreementNumber=orderParam.POAgreementNumber,
                    AgreementNumber = orderParam.AgreementNumber, 
                    OpportunityTypes = type,
                    RequestId = Guid.NewGuid().ToString(),
                    EndCustomerNumber = orderParam.EndCustomerNumber,
                    PublicCustomerNumber=orderParam.PublicCustomerNumber,
                    IncludeInvalidOpportunities = false,
                    PurchaseOrderNumber = null,
                    IsAnnualOrder = false,
                    PageSize=orderParam.PageSize==0?50:orderParam.PageSize,
                    PageNumber=orderParam.PageNumber==0?1:orderParam.PageNumber,
                    SortColumn=orderParam.SortColumn
                } };
            return request;
        }

        /// <summary>
        /// Returns Sales Locations
        /// </summary>

        public async Task<IEnumerable<DomainItem>> GetSalesLocations()
        {
            List<DomainItem> OutputRecords = new List<DomainItem>() { };

            var client = new OrderServiceClient();
            IEnumerable<DomainItemResponse> response;
            response = await client.GetSalesLocationAsync();
            List<DomainItemResponse> resp = response.Cast<DomainItemResponse>().ToList();
            foreach (var item in resp.ToList())
            {
                OutputRecords.Add(Mapper.Map<DomainItemResponse, DomainItem>(item));
            }
            return OutputRecords;
        }

        public async Task<SaveShipmentAddressResult> SaveShipmentAddress(Shipment shipment, string userName)
        {
            var client = new DraftOrderServiceClient();
            var request = new SaveDraftOrderShipmentPortalRequest { Shipment = Mapper.Map<DraftOrderShipment>(shipment), UserName = userName };
            var ret = await client.SaveDraftOrderShipmentPortalAsync(request);
            var response =  Mapper.Map<SaveShipmentAddressResult>(ret);
            response.Errors = response.Errors.EmptyIfNull().ToList<string>();
            return response;
        }

        public async Task<bool> DeleteShipmentAddress(long id)
        {
            var request = new DeleteDraftOrderShipmentPortalRequest() { ShipToId = id };
            var client = new DraftOrderServiceClient();
            var ret = client.DeleteDraftOrderShipmentPortalAsync(request);
            return Convert.ToBoolean(ret.Status);
        }

        public async Task<List<Shipment>> GetAllShipmentList(int agreementId)
        {

            var request = new GetDraftOrderShipmentPortalRequest() { AgreementId = agreementId };
            var client = new DraftOrderServiceClient();
            var res = await client.GetDraftOrderShipmentPortalAsync(request);
            return (res.Shipments.EmptyIfNull().Select(Mapper.Map<Shipment>).ToList());
        }

        public async Task<KeyValuePair<DateTime, List<string>>> ValidateShipToAddress(Shipment validateShipment)
        {
            var request = new ValidateDraftOrderShipmentPortalRequest { Shipment = Mapper.Map<DraftOrderShipment>(validateShipment) };
            var client = new DraftOrderServiceClient();
            var res = await client.ValidateDraftOrderShipmentPortalAsync(request);
            return new KeyValuePair<DateTime, List<string>>(res.LastValidatedDate, res.Error.EmptyIfNull().ToList<string>());
        }


        public static KeyValuePair<DateTime, DateTime> OAPCoverageDateCalculation(string programType, DateTime poliUsageDate, DateTime? currentAnvDt,
            DateTime? endEffectiveDate, DateTime coverageEndDate, string subscriptionMonth, string billinOptionCode)
        {
            #region Calculation Logic
            // i.e. if date has day part ‘dd’ as 1, then the coverage start will be same as usage date
            //if day part is other than 1, then the coverage start will be 1st of next month of usage date
            //If the usage date is in 1st agreement cycle, then the coverage should not span beyond the 
            //1st agreement term irrespective of agreement being renewed or not.
            //For EAS, the coverage will be until the current agreement Anniversary
            //If the order is placed in last month of the agreement such that the coverage is less than a month, 
            //then the coverage start date will be set to 1st of that respective month.

            DateTime startDt = new DateTime();
            DateTime endDt = coverageEndDate;

            DateTime currentAnvDate = currentAnvDt != null
                ? currentAnvDt.GetValueOrDefault().AddMinutes(-1)
                : DateTime.MinValue;

            billinOptionCode = billinOptionCode ?? "0";
            subscriptionMonth = subscriptionMonth ?? "0";
            programType = programType.Trim();

            DateTime nextAnvDt = currentAnvDate.AddYears(1);

            if (currentAnvDt == null)
            {
                nextAnvDt = DateTime.MinValue;
            }

            if (poliUsageDate.Day == 1)
            {
                startDt = poliUsageDate; // the same as usageDate
            }
            else
            {
                if (poliUsageDate.Month == coverageEndDate.Month & poliUsageDate.Year == coverageEndDate.Year &
                    nextAnvDt == DateTime.MinValue)
                {
                    startDt = GetStartDate(poliUsageDate, coverageEndDate, true); // first day of the same month
                }
                else
                {
                    startDt = GetStartDate(poliUsageDate); // first day of the next month
                }
            }

            if (subscriptionMonth == "12")
            {
                endDt = startDt.AddMonths(12).AddDays(-1);
                return new KeyValuePair<DateTime, DateTime>(startDt, endDt);
            }
            if (subscriptionMonth == "36")
            {
                endDt = startDt.AddMonths(36).AddDays(-1);
                return new KeyValuePair<DateTime, DateTime>(startDt, endDt);
            }
            if (subscriptionMonth == "-1")
            {
                endDt = startDt.AddMonths(1).AddDays(-1);
                return new KeyValuePair<DateTime, DateTime>(startDt, endDt);
            }

            if (programType == "EU")
            {
                //For EAS, CT is until the current agreement anniversary, if the usage date is such that it does not leave any coverage in the current anniversary, 
                //then the coverage will be in the next year.
                if (billinOptionCode == "PE" & subscriptionMonth != "12")
                {
                    endDt = endEffectiveDate.GetValueOrDefault();
                }
                else
                {
                    if ((poliUsageDate.Month == currentAnvDate.Month & poliUsageDate.Year == currentAnvDate.Year &
                         nextAnvDt != DateTime.MinValue) ||
                        (poliUsageDate > currentAnvDate & nextAnvDt != DateTime.MinValue))
                    {
                        endDt = nextAnvDt;
                    }
                    else
                    {
                        if (poliUsageDate.Month == currentAnvDate.Month & poliUsageDate.Year == currentAnvDate.Year &
                            nextAnvDt == DateTime.MinValue)
                        {
                            endDt = endEffectiveDate.GetValueOrDefault();
                        }
                        else if (currentAnvDt != null)
                        {
                            endDt = currentAnvDate;
                        }
                        else
                        {
                            endDt = endEffectiveDate.GetValueOrDefault();
                        }
                    }
                }

            }
            else if (programType == "E6" || programType == "USG")
            {
                endDt = endEffectiveDate.GetValueOrDefault();
            }

            return new KeyValuePair<DateTime, DateTime>(startDt, endDt);

            #endregion
        }

        public static DateTime GetStartDate(DateTime poliUsageDate, DateTime enrEndDate = new DateTime(), bool isLastYear = false)
        {
            DateTime startDate = poliUsageDate;

            if (isLastYear)
            {
                startDate = enrEndDate.AddDays(-(enrEndDate.Day - 1)); // first day of the same month as its last year last 30 days
            }
            else
            {
                if (poliUsageDate.Day > 1)
                {
                    startDate = poliUsageDate.AddMonths(1);
                    startDate = startDate.AddDays(-(startDate.Day - 1)); // first of the next month
                }
            }

            return startDate;
        }

        public async Task<SearchResult<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>> GetOrdersWithStatus(  string pcnFilter,
                                                                                                                    string customerNumber, 
                                                                                                                    long? id, 
                                                                                                                    string userName, 
                                                                                                                    int pageNumber, 
                                                                                                                    int pageSize,
                                                                                                                    string[] purchaseOrderStatusCodeTable)
        {
            return await OrderBridge.GetOrdersWithStatus(pcnFilter, customerNumber, id, userName, pageNumber, pageSize, purchaseOrderStatusCodeTable);
        }
    }
}