using AutoMapper;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.Extensions;
using Web.Localization;
using Web.UI.Common;
using Web.UI.Common.Exceptions;
using Web.UI.Common.Extensions;
using Web.UI.Models;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.DraftOrderServiceProxy;
using Web.UI.ServiceGateway.OrderServiceProxy;
using Agreement = Web.UI.Models.Agreement;
using ExcelParser;

namespace Web.UI.UnitOfWork
{
    public class PortalUnitOfWork : IPortalUnitOfWork
    {
        public PortalUnitOfWork(){ }

        [Dependency]
        public IAgreementRepository AgreementRepository { get; set; }
        [Dependency]
        public IProductRepository ProductRepository { get; set; }
        [Dependency]
        public IOrderRepository  OrderRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository { get; set; }

        public async Task<IEnumerable<Agreement>> SearchAgreement(AgreementRequest searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new OrderPortalException("Error_InvalidSearchParam".Localize());
            }
            var ret = await AgreementRepository.SearchOrderableAgreement(searchCriteria);
            return ret.Results.EmptyIfNull().Select(Mapper.Map<Agreement>).ToList();
        }


        public async Task<IEnumerable<Customer>> GetCustomers(AgreementRequest request)
        {
            if (request == null)
            {
                throw new OrderPortalException("Error_InvalidpartnerNumber".Localize());
            }
            var ret = await AgreementRepository.GetCustomers(request);
            return ret.ToList();
        }

        public async Task<Agreement> GetAgreementDetails(string agreementNumber)
        {
            if (agreementNumber.IsNullOrWhiteSpace())
            {
                throw new OrderPortalException("Error_InvalidSearchParam".Localize());
            }
            var ret = await AgreementRepository.GetAgreementDetails(new AgreementDetailsRequest(){AgreementNumbers = new []{agreementNumber}});
            var agr = ret.Results.FirstOrDefault();
            return agr == null ? null:  Mapper.Map<Agreement>(agr);
        }
        public async Task<long> SaveDraftOrder(Agreement agreement, Order order, List<LineItem> orderLineitems, string userName, Guid correlationId)
        { 
            var lineItemList = orderLineitems.EmptyIfNull().Select(Mapper.Map<OrderLineItem>).ToList();

            if (agreement != null && order != null)
            {
                order.AgreementId = agreement.AgreementId;
                order.AgreementNumber = agreement.AgreementNumber;
                order.EndCustomerName = agreement.EndCustomerName;
                order.EndCustomerNumber = agreement.EndCustomerNumber;
                order.ProgramCode = agreement.ProgramCode;
                order.Comments = new List<DraftOrderComment>();
                if (!order.UserComment.IsNullOrWhiteSpace())
                {
                    order.Comments.Add(new DraftOrderComment()
                    {
                        Comment = order.UserComment,
                        Category = "usr",
                        Severity = "5"
                    });
                    order.Comments.Add(new DraftOrderComment() { Comment = order.UserNotes, Category = "NTS", Severity = "5" });
                }
                if (orderLineitems != null && orderLineitems.Count > 0)
                {
                    order.UsageDate = orderLineitems.Min(p => p.POLIUsageDate);
                    order.TotalExtendedAmount = orderLineitems.Sum(p => p.ExtendedAmount);
                }
                foreach (var orderLineItem in lineItemList.EmptyIfNull())
                {
                    if (!orderLineItem.UserComment.IsNullOrWhiteSpace())
                    {
                        var comment = new DraftOrderComment() { Comment = orderLineItem.UserComment, Category = "usr", Severity = "5" };
                        orderLineItem.Comments.Add(comment);
                    }
                }

                var ret = await OrderRepository.SaveDraftOrder(Mapper.Map<OrderHeader>(order), lineItemList, userName, correlationId);
                return ret;
            }
            return -1;
        }

      
        public async Task<long> CreateOrder(Agreement agreement, Models.Order order, List<LineItem> lineItems,
            Shipment shipTo, string userName)
        {
            var result = await OrderRepository.CreateOrder(Mapper.Map<Models.Order, OrderHeader>(order),
                lineItems.Select(Mapper.Map<LineItem, OrderLineItem>).ToList(),null, userName);
            var returnedOrder = result.FirstOrDefault();
            if (returnedOrder != null)
            {
                if (returnedOrder.Status == OrderResponseStatus.Successful)
                {
                    var deletedDraftOrderId = await OrderRepository.DeleteDraftOrder(order.DraftOrderId.GetValueOrDefault(), MyPrincipal.Current.UserName, 0);
                }
                return returnedOrder.Order.Header.PurchaseOrderId;
            }

            return -1;
        }

        public async Task<IEnumerable<KeyValuePair<Order, IEnumerable<LineItem>>>> GetDraftOrder(long? draftOrderId, string pcnFilter, string userName)
        {
            IEnumerable<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>> ret = await OrderRepository.GetDraftOrder(draftOrderId, pcnFilter, userName);

            var result = from KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>> pair in ret.EmptyIfNull()
                         select new KeyValuePair<Order, IEnumerable<LineItem>>
                         (Mapper.Map<Order>(pair.Key), pair.Value.Select(Mapper.Map<LineItem>));
            return result;
        }

        public async Task<IEnumerable<Order>> BulkUploadSave(List<Agreement> agreements, List<PurchaseOrderWithLineItems> excel,
            bool unsubmitflag, string userName)
        {
            //call order header attribute api to get order information
            var results = new List<KeyValuePair<Agreement, OrderHeaderAttributes>>();

            foreach (var item in agreements.EmptyIfNull())
            {
                var retO = await GetOrderHeaderAttributes(item.AgreementId, DateTime.Now, "1033");
                results.Add(new KeyValuePair<Agreement, OrderHeaderAttributes>(item, retO));
            };

            //add agreement information into model
            var model = 
                (from ret in results.EmptyIfNull()
                join item in excel.EmptyIfNull()
                     on ret.Key.AgreementNumber equals item.OrderHeader.AgreementNumber
                 select new
                 {
                     agreement = ret.Key,
                     orderHeaderAttributes = ret.Value,
                     order = Mapper.Map<Order>(item.OrderHeader),
                     lineitems = item.LineItems.Select(Mapper.Map<LineItem>).ToList()
                 }).ToList();

            foreach (var item in model.EmptyIfNull())
            {
                //setup order pricing currency code with agreement currency code
                item.order.PricingCurrencyCode = item.agreement.CurrencyCode;

                //setup order lineitem usage date with order usage date
                foreach (var item2 in item.lineitems.EmptyIfNull())
                {
                    item2.POLIUsageDate = item.order.UsageDate;
                }

                //setup order direct customer number with order header attributes information
                item.order.DirectCustomerNumber = item.orderHeaderAttributes.DirectPartners != null ?
                    (item.orderHeaderAttributes.DirectPartners.Count >= 1 ? item.orderHeaderAttributes.DirectPartners[0].Code : null) : null;
            }

            //get more information for order line item.
            foreach (var item in model.EmptyIfNull())
            {
                foreach (var lineitem in item.lineitems.EmptyIfNull())
                {
                    var request = new ProductRequest
                    {
                        PartNumber = lineitem.PartNumber,
                        ItemName = string.Empty,
                        UsageDate = lineitem.POLIUsageDate ?? DateTime.Now,
                        ProductFamilyCode = string.Empty,
                        PurchaseOrderTypeCode = item.order.PurchaseOrderTypeCode,
                        AgreementId = item.agreement.AgreementId,
                        AgreementNumber = item.agreement.AgreementNumber,
                        PoolIds = item.orderHeaderAttributes.AvailablePoolIds,
                        ProgramOfferings = lineitem.ProgramOfferingCode,
                        ProgramCode = item.agreement.ProgramCode,
                        PageSize = 100,
                        PageNumber = 1
                    };

                    //call search sku api
                    var searchResult = new SearchResult<LineItem>();
                    var ret = await ProductRepository.SearchProducts(request);
                    searchResult.Results = ret.Results.EmptyIfNull().Select(Mapper.Map<LineItem>).ToList();
                    searchResult.TotalCount = ret.TotalCount;

                    if (searchResult.TotalCount > 0)
                    {
                        lineitem.ProductId = searchResult.Results[0].ProductId;
                        lineitem.ItemName = searchResult.Results[0].ItemName;
                        lineitem.LineItemType = "NEW";
                        lineitem.ProductFamilyCode = searchResult.Results[0].ProductFamilyCode;
                        lineitem.ProductTypeCode = searchResult.Results[0].ProductTypeCode;
                        lineitem.IsOlsItem = searchResult.Results[0].IsOlsItem;
                    }

                    //call get coverage date api
                    var ret2 = GetCoverageDate(
                        item.agreement.ProgramCode,
                        lineitem.POLIUsageDate ?? new DateTime(),
                        item.agreement.ReducedAnniversaryDate,
                        item.agreement.EndEffectiveDate,
                        item.agreement.ComplianceEnd ?? new DateTime(),
                        lineitem.PurchaseUnitQuantity,
                        lineitem.BillingOptionCode);

                    lineitem.CoverageStartDate = ret2.Key;
                    lineitem.CoverageEndDate = ret2.Value;

                    //call get estimate api
                    var ret3 = await GetPriceEstimate(lineitem, item.agreement, item.order.PurchaseOrderTypeCode);

                    lineitem.UnitPrice = ret3.SystemPrice ?? 0;
                    lineitem.ExtendedAmount = lineitem.UnitPrice * lineitem.QuantityOrdered;
                }
            }

            //Save draft order and do server side validation
            Guid batchGuid = Guid.NewGuid();
            foreach (var saveUnit in model.EmptyIfNull())
            {
                var retId = await SaveDraftOrder(saveUnit.agreement, saveUnit.order, saveUnit.lineitems, userName, batchGuid);
                saveUnit.order.DraftOrderId = retId > 0? retId : (long?)null;

                var retErrors = await ValidateDraftOrder(saveUnit.agreement, saveUnit.order, saveUnit.lineitems, userName);
                saveUnit.order.Comments = (from error in retErrors.EmptyIfNull()
                              select new DraftOrderComment ()
                              {
                                  Category = "Server Side Validation Error",
                                  Comment = error
                              }).ToList();
            };

            //Submit order
            if (!unsubmitflag)
            {
                foreach (var saveUnit in model.EmptyIfNull())
                {
                    var retId = await CreateOrder(saveUnit.agreement, saveUnit.order, saveUnit.lineitems, null, userName);
                    saveUnit.order.Id = retId > 0 ? retId : (long?)null; //PurchaseOrderId
                }
            }

            return model.Select(n => n.order).ToList();
        }

        public async Task<IEnumerable<LineItem>> SearchHistoryLineItem(int agreementId, string endCustomerNumber)
        {
            var ret = await ProductRepository.GetOpportunitiesByOrderHistory(agreementId, endCustomerNumber);
            return ret.Select(Mapper.Map<LineItem>);
        }

        public async Task<IEnumerable<LineItem>> SearchStepUpOpportunity(OpportunityRequest searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new OrderPortalException("Error_InvalidSearchParam".Localize());
            }
            var ret = await OrderRepository.GetNonResStepUpOpportunties(searchCriteria);
            return ret != null ? ret.Results.SelectMany(r => r.TargetLineItems.Select(Mapper.Map<LineItem>)).ToList() : Enumerable.Empty<LineItem>();
        }

        public async Task<IEnumerable<LineItem>> SearchRenewalOpportunity(OpportunityRequest searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new OrderPortalException("Error_InvalidSearchParam".Localize());
            }

            var ret = await OrderRepository.GetRenewalOpportunties(searchCriteria);
            return ret.Results == null ? Enumerable.Empty<LineItem>() : ret.Results.SelectMany(item => item.TargetLineItems.Select(Mapper.Map<LineItem>)).ToList();
        }

        public async Task<IEnumerable<LineItem>> SearchTrueUpOpportunity(OpportunityRequest searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new OrderPortalException("Error_InvalidSearchParam".Localize());
            }
            var ret = await OrderRepository.GetRenewalOpportunties(searchCriteria);
            return ret == null ? Enumerable.Empty<LineItem>() : ret.Results.SelectMany(item => item.TargetLineItems.Select(Mapper.Map<LineItem>)).ToList();
        }

        public async Task<SearchResult<LineItem>> SearchSku(ProductRequest productRequest)
        {
            var searchResult = new SearchResult<LineItem>();
            var ret = await ProductRepository.SearchProducts(productRequest);
            searchResult.Results = ret.Results.EmptyIfNull().Select(Mapper.Map<LineItem>).ToList();
            searchResult.TotalCount = ret.TotalCount;
            return searchResult;
        }

        public async Task<OrderHeaderAttributes> GetOrderHeaderAttributes(int agreementId, DateTime lookUpDate,string localeCode)
        {
            var orderHeaderAttributes = new OrderHeaderAttributes();
            var responseHeaderAttributes = await OrderRepository.GetOrderHeaderAttributes(agreementId);
            orderHeaderAttributes.IndirectPartners = responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "INDIRECTPARTNER").ToList();
            orderHeaderAttributes.DirectPartners =  responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "DIRECTPARTNER").ToList();
            orderHeaderAttributes.PurchaseOrderTypes = responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "POTYPE").ToList();
            orderHeaderAttributes.AvailablePoolIds = responseHeaderAttributes.Single(h => h.Category.ToUpperInvariant() == "POOLIDS").Name;
            orderHeaderAttributes.ProgramOfferingIds = responseHeaderAttributes.Single(h => h.Category.ToUpperInvariant() == "OFFERINGS").Name;
            return orderHeaderAttributes;
        }


        public async Task<LineItemAttributes> GeLineItemAttributes(LineItemRequest lineItemRequest)
        {
            var orderItemAttributes = new LineItemAttributes();
            LineItemAttributeRequest lineAttributeRequest = new LineItemAttributeRequest();
            lineAttributeRequest.AgreementId = lineItemRequest.AgreementId;
            lineAttributeRequest.LocaleId = lineItemRequest.LocaleId;
            lineAttributeRequest.ProgramCode = lineItemRequest.ProgramCodes.FirstOrDefault();
            lineAttributeRequest.UsageDate = lineItemRequest.LookupDate;
            lineAttributeRequest.ItemId = lineItemRequest.ItemId.ToString();
            lineAttributeRequest.PurchaseOrderTypeCode = lineItemRequest.PurchaseOrderTypes.ToArray().Trim().FirstOrDefault();
            var responseHeaderAttributes = await OrderRepository.GetLineItemAttributes(lineAttributeRequest);
            orderItemAttributes.PurchaseUnitQuantities = responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "PURCHASEUNITQUANTITY").ToList();
            orderItemAttributes.PurchaseUnitTypes = responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "PURCHASEUNITTYPE").ToList();
            orderItemAttributes.ProgramOfferings = responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "PROGRAMOFFERING").ToList();
            DomainItem isOls=responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "ISOLS").FirstOrDefault();
            orderItemAttributes.IsOls = isOls != null ? isOls.Code == "1" ? true : false : false;
            DomainItem isShipment = responseHeaderAttributes.Where(h => h.Category.ToUpperInvariant() == "ISSHIPMENTENABLED").FirstOrDefault();
            orderItemAttributes.IsShipmentEnabled = !orderItemAttributes.IsOls; 
            return orderItemAttributes;
        }
        
        public KeyValuePair<DateTime, DateTime> GetCoverageDate(string programType, DateTime poliUsageDate, DateTime? currentAnvDt,
            DateTime? endEffectiveDate, DateTime coverageEndDate, string subscriptionMonth, string billinOptionCode)
        {
            return Repositories.OrderRepository.OAPCoverageDateCalculation(programType, poliUsageDate, currentAnvDt,
                    endEffectiveDate, coverageEndDate, subscriptionMonth, billinOptionCode);
        }
        
        public async Task<PriceAtNewLevel> GetPriceEstimate(LineItem lineItem, Agreement agreement, string poType)
        {
            return await OrderRepository.GetPriceEstimate(Mapper.Map<OrderLineItem>(lineItem), Mapper.Map<Repositories.DomainModels.Agreement>(agreement), poType);
        }
        
        public async Task<long> LockDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            return await OrderRepository.LockDraftOrder(draftOrderId, userName, maxLockMinutes);
        }

        public async Task<long> UnlockDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            return await OrderRepository.UnlockDraftOrder(draftOrderId, userName, maxLockMinutes);
        }

        public async Task<long> DeleteDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            return await OrderRepository.DeleteDraftOrder(draftOrderId, userName, maxLockMinutes);
        }

        public async Task<List<string>> ValidateDraftOrder(Agreement agreement, Order order, List<LineItem> orderLineitems, string userName)
        {
            var lineItemList = orderLineitems.EmptyIfNull().Select(Mapper.Map<OrderLineItem>).ToList();

            order.AgreementId = agreement.AgreementId;
            order.AgreementNumber = agreement.AgreementNumber;
            order.EndCustomerName = agreement.EndCustomerName;
            order.EndCustomerNumber = agreement.EndCustomerNumber;
            order.ProgramCode = agreement.ProgramCode;
            order.Comments = new List<DraftOrderComment>();
            var comment = new DraftOrderComment() { Comment = order.UserComment, Category = "usr", Severity = "5" };
            order.Comments.Add(comment);
            if (orderLineitems != null && orderLineitems.Count > 0)
            {
                order.UsageDate = orderLineitems.Min(p => p.POLIUsageDate);
        }

            var ret = await OrderRepository.ValidateDraftOrder(Mapper.Map<OrderHeader>(order), lineItemList, userName);
            return ret;
        }

        public async Task<SaveShipmentAddressResult> SaveShipmentAddress(Shipment shipment, string userName)
        {
            return await OrderRepository.SaveShipmentAddress(shipment, userName);
        }

        public async Task<List<Shipment>> GetAllShipmentList(int agreementId)
        {
            var ret = await OrderRepository.GetAllShipmentList(agreementId);
            return ret;
        }

        public async Task<bool> DeleteShipmentAddress(long id)
        {
            var ret = await OrderRepository.DeleteShipmentAddress(id);
            return ret;
        }

        public async Task<KeyValuePair<DateTime, List<string>>> ValidateShipToAddress(Shipment shipment)
        {
            var ret = await OrderRepository.ValidateShipToAddress(shipment);
            return ret;
        }

        public async Task<UserPreference> GetUserPreference(UserPreference request)
        {
            if(request == null)
            {
                throw new OrderPortalException("User Preference is Required");
            }

            var input = new GetUserPreferenceRequest
            {
                ApplicationId = request.ApplicationGuid,
                RequestId = request.RequestId,
                AccessorGuid = request.AccessorGuid,
                ApplicationGuid = request.ApplicationGuid,
                UserCredential = request.UserCredential,
                Module = request.Module

            };
            var ret = await UserRepository.GetUserPreferenceAsync(input);
            return ret;
        }
        public async Task<bool> SetUserPreference(UserPreference request)
        {
            if (request == null)
            {
            throw new OrderPortalException("User Preference is Required");
            }
            
            var input = new SetUserPreferenceRequest
            {
                ApplicationId = request.ApplicationGuid,
                RequestId = request.RequestId,
                AccessorGUID = request.AccessorGuid,
                ApplicationGUID = request.ApplicationGuid,
                UserCredential = request.UserCredential,
                AddressFormat = request.AddressFormat,
                CultureCode = request.Language,
                DateFormat = request.DateFormat,
                Module = request.Module

            };
            var ret = await UserRepository.SetUserPreferenceAsync(input);
            return ret;
        }

        public async Task<SearchResult<KeyValuePair<Order, IEnumerable<LineItem>>>> GetOrdersWithStatus(string pcnFilter,
            string customerNumber, long? id, string userName, int pageNumber, int pageSize, string[] purchaseOrderStatusCodeTable)
        {
            SearchResult<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>> ret = await OrderRepository.GetOrdersWithStatus(pcnFilter, customerNumber, id, userName, pageNumber, pageSize, purchaseOrderStatusCodeTable);

            var result = from KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>> pair in ret.Results.EmptyIfNull()
                         select new KeyValuePair<Order, IEnumerable<LineItem>>
                         (Mapper.Map<Order>(pair.Key), pair.Value.Select(Mapper.Map<LineItem>));
            var totalCount = ret.TotalCount;

            return new SearchResult<KeyValuePair<Order, IEnumerable<LineItem>>>()
            {
                Results = result.ToList(), 
                TotalCount = totalCount
            };
        }
    }
}