using AutoMapper;
using Global.SearchService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Practices.Unity;
using Web.Common.Extensions;
using Web.UI.Repositories.AutoMapper;
using Web.UI.Repositories.Data;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Web.UI.Repositories.Bridges;
using PlainElastic.Net.Queries;
using Global.Search.Strategies;
using OrderServiceProxy=Web.UI.ServiceGateway.OrderServiceProxy;
using Web.UI.ServiceGateway.OrderServiceProxy;
namespace Web.UI.Repositories
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {


        #region Constants
        /// <summary>
        /// Setup Filter For Azure Search (SetupCatalog)
        /// </summary>
        private const string stepupfilter = "$filter=AgreementNumber eq '{0}'";
        /// <summary>
        /// Default Filter For Azure Search(Renewals)
        /// </summary>
        private const string renewalfilter = "$filter=AgreementNumber eq '{0}'";
        /// <summary>
        /// Default Filter For Azure Search(AvailableinPool)
        /// </summary>
        private const string availablepoolfilter = "$filter=ItemID eq '{0}'";
        /// <summary>
        /// Agreement Filter For Azure Search(AvailableinPool)
        /// </summary>
        private const string agreementfilter = "$filter=DirectCustomerNumber eq '{0}' or ( EndCustomerNumber eq '{1}' or CustomerName eq '{2}' or AgreementNumber eq '{3}')";
        /// <summary>
        /// SpecialPricing Filter For Azure Search(AvailableinPool)
        /// </summary>
        private const string specialpricingfilter = "$filter=AgreementID eq '{0}'";
        /// <summary>
        /// Item Filter For Azure Search(AvailableinPool)
        /// </summary>
        private const string itemfilter = "$filter=(ProductTypeCode eq '{0}' or  ProductFamilyCode eq '{1}' or PartNumber eq '{2}') or ItemName eq '{3}'";
        /// <summary>
        /// LicensePool Filter For Azure Search(LicensePool)
        /// </summary>
        private const string licensefilter = "$filter=LicensePoolID eq '{0}'";
        /// <summary>
        /// Search Service
        /// </summary>
        private static readonly Global.Search.ISearchFactory factory = new Global.Search.SearchFactory();
        public Global.Search.Providers.SearchProvider searchProvider;
        public Global.Search.Providers.ElasticSearchProvider elasticProvider;
        #endregion

        public ProductRepository()
        {
            ProductBridge = new ProductsServiceBridge();
            object[] parameters = new object[] { "products", "AABB9C2904CDABEC6B24E8B0F54D9916", TimeSpan.FromMinutes(1) };
            searchProvider = factory.CreateProvider<Global.Search.Providers.SearchProvider, AzureStrategy>(parameters);
            elasticProvider = factory.CreateProvider<Global.Search.Providers.ElasticSearchProvider, AzureStrategy>(parameters);
        }
        public IProductBridge ProductBridge { get; set; }
        public async Task<IEnumerable<OderableItem>> SearchRenewalProducts(string partnerPCN, string customerPCN, string agreementNumber)
        {
            string indexname = ConfigData.RenewalIndex;
            // string filter = string.Format(renewalfilter, agreementNumber);
            String query = string.Format(renewalfilter, agreementNumber);
            IEnumerable<OderableItem> OutputRecords = (await searchProvider.Search<OderableItem>(query.UriEncode(), indexname)).SearchResults;
            return OutputRecords;
        }

        public async Task<IEnumerable<OderableItem>> SearchStepupProducts(string partnerPCN, string customerPCN, string agreementNumber)
        {

            string indexname = ConfigData.StepUpIndex;
            // string filter = string.Format(stepupfilter, agreementNumber);
            String query = string.Format(stepupfilter, agreementNumber);
            IEnumerable<OderableItem> OutputRecords = (await searchProvider.Search<OderableItem>(query.UriEncode(), indexname)).SearchResults;
            return OutputRecords;

        }

        public IEnumerable<OderableItem> SearchProducts(string partnerPCN, string customerPCN, string agreementNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OderableItem> SearchCommitedProducts(string partnerPCN, string customerPCN, string agreementNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AvailableInPool>> SearchAvialablePool(string itemId)
        {
            string indexname = ConfigData.AvailableAppPoolIndex;
            //string filter = string.Format(availablepoolfilter, itemId);
            String query = string.Format(availablepoolfilter, itemId);
            IEnumerable<AvailableInPool> OutputRecords = (await searchProvider.Search<AvailableInPool>(query.UriEncode(), indexname)).SearchResults;
            return OutputRecords;
        }

        public async Task<SearchResult<OrderLineItem>> SearchProducts(ProductRequest productRequest)
        {
            return await ProductBridge.SearchProducts(productRequest);

        }

        public async Task<IEnumerable<AgreementRef>> SearchAgreement(string partnerPCN, string customerPCN, string customerName, string agreementNumber, string UsageDate)
        {
            string indexname = ConfigData.AgreementIndex;
            //   string filter = string.Format(agreementfilter, partnerPCN, customerPCN, customerName, agreementNumber);
            string query = string.Format(agreementfilter, partnerPCN, customerPCN, customerName, agreementNumber);
            IEnumerable<AgreementRef> OutputRecords = (await searchProvider.Search<AgreementRef>(query.UriEncode(), indexname)).SearchResults;
            return OutputRecords;
        }
        public async Task<IEnumerable<SpecialPricing>> SearchSpecialPricing(string agreementId)
        {
            string indexname = ConfigData.SpecialPricingIndex;
            String query = string.Format(specialpricingfilter, agreementId);
            IEnumerable<SpecialPricing> OutputRecords = (await searchProvider.Search<SpecialPricing>(query.UriEncode(), indexname)).SearchResults;
            return OutputRecords;
        }
        public async Task<IEnumerable<LicensePool>> SearchLicense(string licensePoolId)
        {
            string indexname = ConfigData.LicensePoolIndex;
            String query = string.Format(licensefilter, licensePoolId);
            IEnumerable<LicensePool> OutputRecords = (await searchProvider.Search<LicensePool>(query.UriEncode(), indexname)).SearchResults;
            return OutputRecords;
        }
        public async Task<IEnumerable<Offering>> SearchOffering(string agreementId)
        {
            string indexname = ConfigData.OfferingIndex;
            String query = string.Format(specialpricingfilter, agreementId);
            IEnumerable<Offering> OutputRecords = (await searchProvider.Search<Offering>(query.UriEncode(), indexname)).SearchResults;
            return OutputRecords;
        }

       public async Task<IEnumerable<OrderLineItem>> SearchProductsByPartNumber(ProductRequest productRequest)
        {
           List<OrderLineItem> OutputRecords= new List<OrderLineItem>();
            var client = new OrderServiceClient();
            var orderRequest = new SearchOrderableVLProductsRequest
            {
                RequestId =ConfigData.OrderRequestId,
                ApplicationId =ConfigData.ApplicationId,
                SortingDescending = false,
                AgreementNumber = productRequest.AgreementNumber, 
                PageIndex =  1,
                PageSize = 5,
                PurchaseOrderTypeCode =productRequest.PurchaseOrderTypeCode,
                ProgramOfferingCodes = new string[] { productRequest.ProgramOfferings },
                PCN = productRequest.PartNumber,
                LocaleId =productRequest.LocaleId,
                OrgGuid = string.Empty,
                Program =productRequest.ProgramCode
                 
            };
            List<OrderServiceProxy.SearchCriteria> searchCriteriaList = new List<OrderServiceProxy.SearchCriteria>();
            var searchCriteria = new OrderServiceProxy.SearchCriteria
            {
                Name =
                    (OrderServiceProxy.SearchCriteriaName)
                    (Enum.Parse(typeof(SearchCriteriaName),
                                SearchType.PartNumber.ToString())),
                Value = productRequest.PartNumber    
            };
            searchCriteriaList.Add(searchCriteria);
            OrderServiceProxy.SearchCriteria[] searchcriteria=searchCriteriaList.ToArray();
            orderRequest.AdditionalSearchCriterion = (searchcriteria);
            var response = new SearchOrderableProductsResponse();
            response = await client.SearchOrderableProductsAsync(orderRequest);
            OutputRecords.AddRange(Mapper.Map<IEnumerable<ProductPriceDetail>, IEnumerable<OrderLineItem>>(response.ProductPriceDetail.ToList()));
            OutputRecords.AddRange(Mapper.Map<IEnumerable<ProductBase>, IEnumerable<OrderLineItem>>(response.ProductSummaries.ToList()));
            return OutputRecords;
        }
       public async Task<IEnumerable<OrderLineItem>> GetOpportunitiesByOrderHistory(int agreementId, string endCustomerNumber)
       {
           return (await ProductBridge.GetOpportunitiesByOrderHistory(agreementId, endCustomerNumber)).Results;
       }

       public enum SearchType
       {
           PartNumber,
           ItemName,
           ProductFamilyName,
           AgreementNumber,
           CustomerName,
           OrganizationGUID
       }


    }
}
