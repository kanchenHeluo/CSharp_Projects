using AutoMapper;
using PlainElastic.Net.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Web.UI.Common.Extensions;
using Global.Search.Strategies;
using Web.UI.Repositories.Data;

namespace Web.UI.Repositories.Bridges
{
    public class NoSqlAgreementBridge : IAgreementBridge
    {
        #region NoSqlProvider
        public Global.Search.Providers.ElasticSearchProvider elasticProvider;
        private static readonly Global.Search.ISearchFactory factory = new Global.Search.SearchFactory();
        public Global.Search.Providers.SearchProvider azureProvider;
        #endregion
        #region Intialization

        public NoSqlAgreementBridge()
        {
            object[] parameters = new object[] { "agreements", "EDB114F3C804F6D788F7B077B3435A81", TimeSpan.FromMinutes(1) };
            azureProvider = factory.CreateProvider<Global.Search.Providers.SearchProvider, AzureStrategy>(parameters);
            elasticProvider = new Global.Search.Providers.ElasticSearchProvider(null);
        }
        #endregion
        #region NoSQLImplementation
        /// </summary>
        /// <param name="agreementRequest"></param>
        /// <returns></returns>
        public async Task<SearchResult<Agreement>> SearchOrderableAgreement(AgreementRequest agreementRequest)
        {
            List<Agreement> agreements = new List<Agreement>();
            SearchResult<Agreement> OutputRecords = new SearchResult<Agreement>();
            string indexname;
            string query;
            if (!ConfigData.NoSQLBridgeSwitch)
            {
                if (((agreementRequest.AgreementNumber != null || agreementRequest.EndCustomerName != null || agreementRequest.PartnerNumber != null || agreementRequest.SalesLocation != null || agreementRequest.EndCustomerNumber != null) && agreementRequest.LookUpDate != null))
                {

                    indexname = "searchagreement";

                    query = new QueryBuilder<SearchAgreements>()
                               .Query(q => q
                                   .Bool(b => b
                                       .Must(m => m.
                                            Term(t => t.Field(user => user.EndCustomerName).Value(agreementRequest.EndCustomerName.With(s => s.ToLower())))
                                               .Match(mt => mt.Field(agg => agg.POAgreementNumber).Query(agreementRequest.AgreementNumber))
                                               .Match(mt => mt.Field(agg => agg.PartnerPCN).Query(agreementRequest.PartnerNumber.With(s => s.ToLower())))
                                               .Match(mt => mt.Field(agg => agg.EndCustomerNumber).Query(agreementRequest.EndCustomerNumber.With(s => s.ToLower())))
                                               .Range(mt => mt.Field(agg => agg.AgreementStartDate).Lte(agreementRequest.LookUpDate.ToString("yyyy-MM-dd")))
                                               .Range(mt => mt.Field(agg => agg.AgreementEndDate).Gte(agreementRequest.LookUpDate.ToString("yyyy-MM-dd")))
                                              ).Must(m => m.Term(t => t.Field(agg => agg.SalesLocationCode).Value(agreementRequest.SalesLocation.With(s => s.Trim()))))


                                              )).BuildBeautified();

                    IEnumerable<SearchAgreements> ServiceOutput = elasticProvider.Search<SearchAgreements>(query, indexname, "agreements").SearchResults;
                    agreements.AddRange(Mapper.Map<IEnumerable<SearchAgreements>, IEnumerable<Agreement>>(ServiceOutput));
                    OutputRecords.Results = agreements;
                    OutputRecords.TotalCount = agreements.Count();

                }

            }
            else if (ConfigData.NoSQLBridgeSwitch)
            {
                if (((agreementRequest.AgreementNumber != null || agreementRequest.EndCustomerName != null || agreementRequest.PartnerNumber != null || agreementRequest.SalesLocation != null || agreementRequest.EndCustomerNumber != null) && agreementRequest.LookUpDate != null))
                {
                    NoSqlAgreementBridge bridge = new NoSqlAgreementBridge();
                    indexname = "newagreements";
                    string filter = "$filter=POAgreementNumber+eq+'" + agreementRequest.AgreementNumber + "'+and+EndCustomerNumber+eq+'" + agreementRequest.EndCustomerNumber + "'+and+SalesLocationCode+eq+'" + agreementRequest.SalesLocation + "'+and+AgreementStartDate+ge+" + agreementRequest.LookUpDate.ToString("yyyy-MM-dd'T'00:00:00'Z'") + "";
                    //   string filter = "$filter=IsAffiliate+eq+'T'";
                    string searchFields = "$top=5000";
                    // filter = AzureQueryBuilder(filter, FilterColumns.AvailableInPoolID, agreementRequest.PoolIds);
                    // filter = AzureQueryBuilder(filter, FilterColumns.ProgramOfferingCode, productRequest.ProgramOfferings);
                    // filter = productRequest.ProductTypeCode != null ? AzureQueryBuilder(filter, FilterColumns.ProductTypeCode, productRequest.ProductTypeCode) : filter;
                    //filter = productRequest.ProductFamilyCode != null ? AzureQueryBuilder(filter, FilterColumns.ProductFamilyCode, productRequest.ProductFamilyCode) : filter;
                    //searchFields = productRequest.SearchText != null ? AzureQueryBuilder(searchFields, FilterColumns.SearchParam, productRequest.SearchText) : filter;
                    NoSqlAgreementBridge pros = new NoSqlAgreementBridge();
                    IEnumerable<SearchAgreements> ServiceOutput = (await azureProvider.Search<SearchAgreements>(filter, indexname, searchFields, "")).SearchResults; ;
                    agreements.AddRange(Mapper.Map<IEnumerable<SearchAgreements>, IEnumerable<Agreement>>(ServiceOutput));
                    DateTime lookupdate = agreementRequest.LookUpDate;
                    OutputRecords.Results = agreements;
                    OutputRecords.TotalCount = agreements.Count();
                }
            }
            return OutputRecords;
        }

        //private string AzureQueryBuilder(string filter, FilterColumns filterColumns, string data)
        //{
        //    string[] dataItems = data.Split(',');
        //    if (dataItems.Count() > 1)
        //    {
        //        foreach (string item in dataItems)
        //        {
        //            if (filter == ConfigData.ProductsFilter)
        //            {
        //                filter = filter + filterColumns + "/any(s:s+eq+'" + item + "')";
        //            }
        //            else
        //            {
        //                filter = filter + "+and+" + filterColumns + "/any(s:s+eq+'" + item + "')";
        //            }
        //        }
        //    }
        //    else if (filterColumns == FilterColumns.SearchParam)
        //    {
        //        filter = filter + dataItems[0] + "*" + "&searchFields=" + FilterColumns.PartNumber + "," + FilterColumns.ItemName; //+ "," + FilterColumns.ProductFamilyName;
        //    }
        //    else
        //    {
        //        filter = filter + "+and+" + filterColumns + "+eq+'" + dataItems[0] + "'";
        //    }
        //    return filter;
        //}
        //public enum FilterColumns
        //{
        //    POAgreementNumber,
        //    EndCustomerNumber,
        //    SalesLocationCode,
        //    AgreementStartDate

        //}


        /// <summary>
        #endregion
    }
}
