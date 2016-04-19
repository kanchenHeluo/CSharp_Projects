using AutoMapper;
using Global.Search.Strategies;
using PlainElastic.Net.Queries;
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

namespace Web.UI.Repositories.Bridges
{
    public class NoSqlProductsBridge : IProductBridge
    {
        #region NoSqlProviders
        public Global.Search.Providers.ElasticSearchProvider elasticsearchProvider;
        private static readonly Global.Search.ISearchFactory factory = new Global.Search.SearchFactory();
        public Global.Search.Providers.SearchProvider azureProvider;
        #endregion

        #region Intialization
        public NoSqlProductsBridge()
        {
            object[] parameters = new object[] { "productcatalog", "06C5F6D0F0A6729B4B3A79E77C522337", TimeSpan.FromMinutes(1) };
            azureProvider = factory.CreateProvider<Global.Search.Providers.SearchProvider, AzureStrategy>(parameters);
            elasticsearchProvider = new Global.Search.Providers.ElasticSearchProvider(null);
        }
        #endregion
     
        #region NoSQLImplementation
        public async Task<SearchResult<OrderLineItem>> SearchProducts(ProductRequest productRequest)
        {
            List<OrderLineItem> orderItems = new List<OrderLineItem>();
            SearchResult<OrderLineItem> OutputRecords = new SearchResult<OrderLineItem>();
            string indexname;
            string query;
            if (!ConfigData.NoSQLBridgeSwitch)
            {
                indexname = "products";
                query = "";
                #region ElasticQuerybuilder
                if (productRequest.SearchPattern == SearchPattern.Like)
                {

                    query = new QueryBuilder<OrderItem>()
                          .Query(q => q
                              .Bool(b => b
                                  .Must(m => m.
                                       Terms(mt => mt.Field(agg => agg.AvailableInPoolID).Values(productRequest.PoolIds != null ? productRequest.PoolIds.ToLower().Split(",".ToArray()) : productRequest.PoolIds.Split(",".ToArray())).MinimumMatch(productRequest.PoolIds.Split(",".ToArray()).Count())))
                                       .Must(m => m.Terms(mt => mt.Field(agg => agg.ProgramOfferingCode).Values(productRequest.ProgramOfferings != null ? productRequest.ProgramOfferings.ToLower().Split(",".ToArray()) : productRequest.ProgramOfferings.Split(",".ToArray())).MinimumMatch(productRequest.ProgramOfferings.Split(",".ToArray()).Count())))
                                       .Must(m => m.Match(mt => mt.Field(agg => agg.ProductTypeCode).Query(productRequest.ProductTypeCode)))
                                       .Must(m => m.Match(mt => mt.Field(agg => agg.ProductFamilyCode).Query(productRequest.ProductFamilyCode)))
                                       .Must(m => m.MultiMatch(t => t.Query(productRequest.SearchText != null ? productRequest.SearchText.ToLower() : productRequest.SearchText).Fields(prod => prod.PartNumber, prod => prod.ItemName,prod=>prod.ProductFamilyName)))
                                         )).Size(productRequest.PageSize)
                                         .From(productRequest.PageNumber == 1 ? productRequest.PageNumber : productRequest.PageNumber * productRequest.PageSize)
                                        .BuildBeautified();
                }
                else if (productRequest.SearchPattern == SearchPattern.Exact)
                {
                    query = new QueryBuilder<OrderItem>()
                         .Query(q => q
                             .Bool(b => b
                                 .Must(m => m.
                                      Match(t => t.Field(prod => prod.PartNumber).Query(productRequest.PartNumber != null ? productRequest.PartNumber.ToLower() : productRequest.PartNumber))
                                      .Terms(mt => mt.Field(agg => agg.ProgramOfferingCode).Values(productRequest.PoolIds != null ? productRequest.PoolIds.ToLower().Split(",".ToArray()) : productRequest.PoolIds.Split(",".ToArray())).MinimumMatch(productRequest.PoolIds.Split(",".ToArray()).Count()))
                                        .Terms(mt => mt.Field(agg => agg.ProgramOfferingCode).Values(productRequest.ProgramOfferings != null ? productRequest.ProgramOfferings.ToLower().Split(",".ToArray()) : productRequest.ProgramOfferings.Split(",".ToArray())).MinimumMatch(productRequest.ProgramOfferings.Split(",".ToArray()).Count()))
                                         .Match(mt => mt.Field(agg => agg.ProductTypeCode).Query(productRequest.ProductTypeCode != null ? productRequest.ProductTypeCode.ToLower() : productRequest.ProductTypeCode))
                                          .Match(mt => mt.Field(agg => agg.ProductFamilyCode).Query(productRequest.ProductFamilyCode != null ? productRequest.ProductFamilyCode.ToLower() : productRequest.ProductFamilyCode))
                                        )
                                        ))
                                        .BuildBeautified();
                }
                #endregion
                IEnumerable<OrderItem> ServiceOutput = elasticsearchProvider.Search<OrderItem>(query, indexname, "productcatalog").SearchResults;
                orderItems.AddRange(Mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderLineItem>>(ServiceOutput));
            }
            else if (ConfigData.NoSQLBridgeSwitch)
            {
                
                indexname = ConfigData.ProductsIndex;
                string filter = ConfigData.ProductsFilter;
                string searchFields = ConfigData.SearchFields;
                filter = AzureQueryBuilder(filter, FilterColumns.AvailableInPoolID, productRequest.PoolIds);
                filter = AzureQueryBuilder(filter, FilterColumns.ProgramOfferingCode, productRequest.ProgramOfferings);
                filter = productRequest.ProductTypeCode != null ? AzureQueryBuilder(filter, FilterColumns.ProductTypeCode, productRequest.ProductTypeCode) : filter;
                filter = productRequest.ProductFamilyCode != null ? AzureQueryBuilder(filter, FilterColumns.ProductFamilyCode, productRequest.ProductFamilyCode) : filter;
                searchFields = productRequest.SearchText != null ? AzureQueryBuilder(searchFields, FilterColumns.SearchParam, productRequest.SearchText) : filter;
                NoSqlProductsBridge pros=new NoSqlProductsBridge();
                IEnumerable<OrderItem> ServiceOutput = (await azureProvider.Search<OrderItem>(filter, indexname,searchFields,"")).SearchResults; ;
                orderItems.AddRange(Mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderLineItem>>(ServiceOutput));

            }

            OutputRecords.Results = orderItems;
            OutputRecords.TotalCount = orderItems.Count();
            return OutputRecords;

        }

        public async Task<SearchResult<OrderLineItem>> GetOpportunitiesByOrderHistory(int agreementId, string endCustomerNumber)
        {
            throw new NotImplementedException();
        }
        #endregion
        private string AzureQueryBuilder(string filter, FilterColumns filterColumns, string data)
        {
            string[] dataItems = data.Split(',');
            if (dataItems.Count() > 1)
            {
                foreach (string item in dataItems)
                {
                    if (filter == ConfigData.ProductsFilter)
                    {
                        filter = filter + filterColumns + "/any(s:s+eq+'" + item + "')";
                    }
                    else
                    {
                        filter = filter + "+and+" + filterColumns + "/any(s:s+eq+'" + item + "')";
                    }
                }
            }
            else if (filterColumns == FilterColumns.SearchParam)
            {
                filter = filter + dataItems[0] + "*" + "&searchFields=" + FilterColumns.PartNumber + "," + FilterColumns.ItemName; //+ "," + FilterColumns.ProductFamilyName;
            }
            else
            {
                filter = filter + "+and+" + filterColumns + "+eq+'" + dataItems[0] + "'";
            }
            return filter;
        }
        

    }

    public enum FilterColumns
    {
        AvailableInPoolID,
        SearchParam,
        PartNumber,
        ItemName,
        ProductFamilyName,
        ProductTypeCode,
        ProductFamilyCode,
        ProgramOfferingCode
    }
}
   
