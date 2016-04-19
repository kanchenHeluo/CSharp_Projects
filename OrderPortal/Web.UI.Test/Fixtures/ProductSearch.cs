using fit;
using Newtonsoft.Json;
using System;
using System.Linq;
using Web.UI.Repositories;
using Web.UI.Repositories.DomainModels;
namespace Web.UI.Test.Fixtures
{
    public abstract class ProductSearchBaseFixture : ColumnFixture
    {
        protected ProductSearchBaseFixture()
        {
            AutoMapperConfig.RegisterMappings();
        }
    }

    public class SearchProducts : ProductSearchBaseFixture
    {
        public string PartNumber { get; set; }
        public int AgreementId { get; set; }

        public string ItemName { get; set; }

        public string ProductFamilyCode { get; set; }
        public string PurchaseOrderTypeCode { get; set; }

     
        public int HasResult()
        {
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
         
           
            ProductRepository newproduct = new ProductRepository();
            ProductRequest productRequest = new ProductRequest();
            productRequest.PartNumber =PartNumber ;
            productRequest.AgreementId = AgreementId;
            productRequest.ItemName=ItemName;
            productRequest.ProductFamilyCode = ProductFamilyCode;
            productRequest.PurchaseOrderTypeCode = PurchaseOrderTypeCode;
            productRequest.UsageDate = dt;
            productRequest.PageSize=100;
            productRequest.PageNumber=1;
            productRequest.SortColumn="ItemName";
            productRequest.IncludeDetails = true;
            var outputTask = newproduct.SearchProducts(productRequest);
            outputTask.Wait();


            return outputTask.Result.TotalCount ;
        }
    }
    public class GetOpportunitiesByOrderHistory : ProductSearchBaseFixture
    {
        public string EndCustomerNumber { get; set; }
        public int AgreementId { get; set; }

        public int HasResult()
        {
            AgreementRepository agr = new AgreementRepository();
            ProductRepository newproduct = new ProductRepository();
            var outputTask = newproduct.GetOpportunitiesByOrderHistory(AgreementId, EndCustomerNumber);
            outputTask.Wait();
            return outputTask.Result.Count();
        }
    }

  
}
