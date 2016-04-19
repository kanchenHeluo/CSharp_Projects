using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.Data;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using OrderServiceProxy = Web.UI.ServiceGateway.OrderServiceProxy;
using Web.UI.ServiceGateway.OrderServiceProxy;

namespace Web.UI.Repositories.Bridges
{
    public class ProductsServiceBridge:IProductBridge
    {

        public async Task<SearchResult<OrderLineItem>> SearchProducts(ProductRequest productRequest)
        {
            List<OrderLineItem> OrderItems = new List<OrderLineItem>();
            SearchResult<OrderLineItem> Output = new SearchResult<OrderLineItem>();
            var client = new OrderServiceClient();
            SearchProductRequest searchProductRequest = new SearchProductRequest
            {
                AgreementId = productRequest.AgreementId,
                PartNumber = productRequest.PartNumber,
                ItemName = productRequest.ItemName,
                ProductFamilyCode = productRequest.ProductFamilyCode,
                PurchaseOrderTypeCode = productRequest.PurchaseOrderTypeCode,
                IncludeDetails = productRequest.IncludeDetails,
                UsageDate = productRequest.UsageDate,
                PageNumber=productRequest.PageNumber,
                PageSize=productRequest.PageSize,
                SortColumn=productRequest.SortColumn
            
            };


            var response =new SearchProductResponse();
            response = await client.GetProductsAsync(searchProductRequest);
            OrderItems.AddRange(Mapper.Map<List<ProductsList>, IEnumerable<OrderLineItem>>(response.Products.ToList()));
            Output.Results = OrderItems;
            Output.TotalCount = response.TotalCount;
            return Output;
        }

        public async Task<SearchResult<OrderLineItem>> GetOpportunitiesByOrderHistory(int agreementId, string endCustomerNumber)
        {
            var client = new OrderServiceClient();
            List<OrderLineItem> GetPurchaseOrders = new List<OrderLineItem>();
            SearchResult<OrderLineItem> Output = new SearchResult<OrderLineItem>();
            var response = new List<SearchPurchaseOpportunitiesResponse>();
            response = client.SearchPurchaseOpportunitiesAsync(agreementId, endCustomerNumber).Result.ToList();
            GetPurchaseOrders.AddRange(Mapper.Map<List<SearchPurchaseOpportunitiesResponse>, IEnumerable<OrderLineItem>>(response.ToList()));
            Output.Results = GetPurchaseOrders;
            Output.TotalCount = GetPurchaseOrders.Count();
            return Output;
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
