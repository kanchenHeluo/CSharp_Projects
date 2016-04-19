using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Common.Extensions;
using Web.UI.Repositories.Data;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.OrderServiceProxy;
namespace Web.UI.Repositories.Bridges
{    public class OrderServiceBridge: IOrderBridge
    {
        public async Task<SearchResult<OpportunityItem>> GetNonResStepUpOpportunties(OpportunityRequest orderParam)
        {
            throw new NotImplementedException();
        }


        public async Task<SearchResult<OpportunityItem>> GetRenewalOpportunties(OpportunityRequest orderParam)
        {
            throw new NotImplementedException();

        }
      /// <summary>
    /// 
    /// </summary>
    /// <param name="orderParam"></param>
    /// <param name="type"></param>
    /// <returns></returns>
        private SearchResult<OpportunityItem> GetOpportunityItem(OpportunityRequest orderParam, OpportunityType type)
        {
            List<OpportunityItem> ItemList = new List<OpportunityItem>() { };
            SearchResult<OpportunityItem> Output = new SearchResult<OpportunityItem>();
            Output.Results = new List<OpportunityItem>();
            OpportunityItem objOpportunities = new OpportunityItem();

            if (orderParam.AgreementNumber.Any())
            {
                var client = new OrderServiceClient();
                
                GetOpportunityResponse[] response;
                response = client.GetOpportunities(BuildOpportunityRequest(orderParam, type).ToArray());
                IEnumerable<GetOpportunityResponse> resp = response.Cast<GetOpportunityResponse>().ToList();
                int index;
                foreach (var oppCont in response.ToList())
                {
                    index = response.ToList().IndexOf(oppCont);

                    foreach (var opp in oppCont.OpportunityContainers.ToList())
                    {
                        objOpportunities.SourceLineItems = new List<OpportunityLineItem>();
                        objOpportunities.TargetLineItems = new List<OpportunityLineItem>();
                        var sourceList = (opp.Opportunities.Select(c => c.SourceLineItems)).ToList();
                        var targetList = (opp.Opportunities.Select(c => c.TargetLineItems)).ToList();
                        int count = sourceList.Count;
                        count = targetList.Count;
                        opp.Opportunities.ToList().ForEach(op =>
                        {
                            op.TargetLineItems.ToList().ForEach(li =>
                            {
                                var opLineItem = Mapper.Map<VLLineItem, OpportunityLineItem>(li);
                                opLineItem.ParentPartNumber = (op.SourceLineItems.Length > 0 ? op.SourceLineItems[0].ItemDetails.Number : null);
                                opLineItem.ParentItemName = (op.SourceLineItems.Length > 0 ? op.SourceLineItems[0].ItemDetails.Name : null);
                                objOpportunities.TargetLineItems.Add(opLineItem);
                                Output.TotalCount = Convert.ToInt32(li.TotalCount);
                                if (Output.TotalCount == null)
                                {
                                    Output.TotalCount = Convert.ToInt32(op.SourceLineItems.Length > 0 ? op.SourceLineItems[0].TotalCount : 0);
                                }
                            });

                        });
                      
                        Output.Results.Add(objOpportunities);
                    }

                }
                return Output;

            }

            else
            {
                return Output;
            }
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
                    PageSize=orderParam.PageSize,
                    PageNumber=orderParam.PageNumber,
                    SortColumn=orderParam.SortColumn
                    
                 

                } };
            return request;
        }

    /// <summary>
    /// Get Completed PurchaseOrderInDetails from Order Service
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public async Task<SearchResult<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>> GetOrdersWithStatus(  string pcnFilter,
                                                                                                                string customerNumber, 
                                                                                                                long? id, 
                                                                                                                string userName, 
                                                                                                                int pageNumber, 
                                                                                                                int pageSize,
                                                                                                                string[] purchaseOrderStatusCodeTable)
        {
            var request = new SearchPurchaseOrderDetailsRequest()
            {
                PurchaseOrderId = (int?)(id),
                DirectCustomerPCN = pcnFilter,
                EndCustomerNumber = customerNumber,
                PurchaseOrderStatusCodeTable = purchaseOrderStatusCodeTable,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var client = new OrderServiceClient();
            var ret = await client.SearchPurchaseOrderWithDetailsAsync(request);
            return new SearchResult<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>()
            {
                Results = ret.Results.Select(item => new KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>(Mapper.Map<OrderHeader>(item), item.LineItems.EmptyIfNull().Select(Mapper.Map<OrderLineItem>))).ToList(),
                TotalCount = ret.TotalCount
            };
        }

    }
}
