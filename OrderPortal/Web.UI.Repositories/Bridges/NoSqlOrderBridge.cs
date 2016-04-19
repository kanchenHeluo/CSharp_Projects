using AutoMapper;
using Global.Search.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.Data;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Web.Common.Extensions;
using Web.UI.ServiceGateway.OrderServiceProxy;

namespace Web.UI.Repositories.Bridges
{
    public class NoSqlOrderBridge:IOrderBridge
    {
         #region azureSearviceProvider
        private static readonly Global.Search.ISearchFactory factory = new Global.Search.SearchFactory();
        public Global.Search.Providers.SearchProvider searchProvider;

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public NoSqlOrderBridge()
        {
            object[] parameters = new object[] { "products", "AABB9C2904CDABEC6B24E8B0F54D9916", TimeSpan.FromMinutes(1) };
            searchProvider = factory.CreateProvider<Global.Search.Providers.SearchProvider, AzureStrategy>(parameters);
        }
     
        #region AzureOrderRepository
        public async Task<SearchResult<OpportunityItem>> GetNonResStepUpOpportunties(OpportunityRequest orderParam)
        {
            List<OpportunityItem> ItemList = new List<OpportunityItem>() { };
            SearchResult<OpportunityItem> Output = new SearchResult<OpportunityItem>();
            OpportunityItem objOpportunities = new OpportunityItem();
            if (ConfigData.NoSQLBridgeSwitch)
            {
                if (orderParam.AgreementNumber.Any())
                {

                    string indexname = ConfigData.SetupSearch;
                    String query = string.Format(ConfigData.OrderFilter, orderParam.AgreementNumber, orderParam.EndCustomerNumber, orderParam.POAgreementNumber);
                    IEnumerable<StepupSearch> ServiceOutput = (await searchProvider.Search<StepupSearch>(query.UriEncode(), indexname)).SearchResults;
                    objOpportunities.SourceLineItems = new List<OpportunityLineItem>();
                    objOpportunities.TargetLineItems = new List<OpportunityLineItem>();
                    var sourceList = ServiceOutput.Where(c => c.LineItemTypeCode.Equals("BAS")).ToList();
                    var targetList = (ServiceOutput.Where(c => c.LineItemTypeCode != "BAS").ToList());
                    objOpportunities.SourceLineItems.AddRange(Mapper.Map<List<StepupSearch>, IEnumerable<OpportunityLineItem>>(sourceList.ToList()));
                    objOpportunities.TargetLineItems.AddRange(Mapper.Map<List<StepupSearch>, IEnumerable<OpportunityLineItem>>(targetList.ToList()));
                    ItemList.Add(objOpportunities);
                    Output.Results = ItemList;
                    Output.TotalCount = ItemList.Count();
                    

                }
               
            }
            else if (!ConfigData.NoSQLBridgeSwitch)
            {
                //this Section is for Elastic Search
               
            }
            return Output;

        }
        //public async Task<SearchResult<OpportunityItem>> GetRenewalOpportunties(OpportunityRequest orderParam)
        //{
        //    List<OpportunityItem> ItemList = new List<OpportunityItem>() { };
        //    SearchResult<OpportunityItem> Output = new SearchResult<OpportunityItem>();
        //    OpportunityItem objOpportunities = new OpportunityItem();
        //    if (ConfigData.NoSQLBridgeSwitch)
        //    {
        //        if (orderParam.AgreementNumber != null)
        //        {

        //            string indexname = ConfigData.GetRenewals;
        //            String query = string.Format(ConfigData.OrderFilter, orderParam.AgreementNumber, orderParam.EndCustomerNumber, orderParam.POAgreementNumber);
        //            IEnumerable<StepupSearch> ServiceOutput = searchProvider.Search<StepupSearch>(query.UriEncode(), indexname).SearchResults;
        //            objOpportunities.SourceLineItems = new List<OpportunityLineItem>();
        //            objOpportunities.TargetLineItems = new List<OpportunityLineItem>();
        //            var sourceList = ServiceOutput.Where(c => c.LineItemTypeCode.Equals("BAS")).ToList();
        //            var targetList = (ServiceOutput.Where(c => c.LineItemTypeCode != "BAS").ToList());
        //            objOpportunities.SourceLineItems.AddRange(Mapper.Map<List<StepupSearch>, IEnumerable<OpportunityLineItem>>(sourceList.ToList()));
        //            objOpportunities.TargetLineItems.AddRange(Mapper.Map<List<StepupSearch>, IEnumerable<OpportunityLineItem>>(targetList.ToList()));
        //            ItemList.Add(objOpportunities);
        //            Output.Results = ItemList;
        //            Output.TotalCount = ItemList.Count();
                 
        //        }
        //        else
        //        {
        //            return Output;
        //        }
        //    }
        //    else if (!ConfigData.NoSQLBridgeSwitch)
        //    {
        //     //This is For Elastic Search Code   
        //    }
        //    return Output;
        //}


        public async Task<SearchResult<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>> GetOrdersWithStatus(
                                                                                                                  string pcnFilter,
                                                                                                                  string customerNumber,
                                                                                                                  long? id,
                                                                                                                  string userName,
                                                                                                                  int pageNumber,
                                                                                                                  int pageSize,
                                                                                                                  string[] purchaseOrderStatusCodeTable)
        {
            throw new NotImplementedException();
        }

        #endregion



    }
}
