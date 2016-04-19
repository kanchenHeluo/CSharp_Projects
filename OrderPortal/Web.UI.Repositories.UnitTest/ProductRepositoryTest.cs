using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.UI.Repositories;
using Web.UI.Repositories.Models;
using System.Collections;
using Web.UI.Repositories.DomainModels;
using System.Collections.Generic;
namespace Web.UI.Repositories.UnitTest
{
    [TestClass]
    public class ProductRepositoryTest
    {
        [TestMethod]
        public void RenewalTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchRenewalProducts("5918671", "975366CD", "8248012");
            Assert.IsNotNull(results);

        }

        [TestMethod]
        public void StepUpTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchStepupProducts("5918671", "975366CD", "5918671");
            Assert.IsNotNull(results);


        }

        [TestMethod]
        public void AvailableinPoolTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchAvialablePool("662310");
            Assert.IsNotNull(results);


        }
        [TestMethod]
        public void AgreementTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchAgreement("9D370B2F", "15759383", "Hana Bank", "01E60004", "Date");
            Assert.IsNotNull(results);


        }

        [TestMethod]
        public void SpecialPricingTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchSpecialPricing("1676910");
            Assert.IsNotNull(results);


        }

        [TestMethod]
        public void ItemTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchProducts(new ProductRequest());
            Assert.IsNotNull(results);


        }
        [TestMethod]
        public void LicenseTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchLicense("12");
            Assert.IsNotNull(results);


        }
        [TestMethod]
        public void OfferingTestMethod()
        {
            ProductRepository newproduct = new ProductRepository();
            var results = newproduct.SearchOffering("2732812");
            Assert.IsNotNull(results);


        }

        [TestMethod]
        public void SearchProductsAzureTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            ProductRepository newproduct = new ProductRepository();
            ProductRequest productRequest = new ProductRequest();
         //   productRequest.ItemName = "languages";
            productRequest.SearchText = "access";
            productRequest.PoolIds = "230820,2939543";
            productRequest.ProgramOfferings = "FUL,ESP";
            productRequest.ProductTypeCode = "DS";
            productRequest.ProductFamilyCode = "CIR";
       
            productRequest.SearchPattern = SearchPattern.Like;
            var results = newproduct.SearchProducts(productRequest);
            Assert.IsNotNull(results.Result);


        }
        [TestMethod]
        public void SearchProductsElasticTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
           ProductRepository newproduct = new ProductRepository();
            ProductRequest productRequest = new ProductRequest();
           // productRequest.SearchText = "languages";
            productRequest.SearchText = "access";
            productRequest.PoolIds = "513301,548836";
            productRequest.ProgramOfferings = "ACP,CUS";
            productRequest.ProductTypeCode = "LSA";
            productRequest.ProductFamilyCode = "CIR"; 
            productRequest.PageSize = 10;
            productRequest.PageNumber = 1;
            productRequest.SearchPattern = SearchPattern.Like;
            var results = newproduct.SearchProducts(productRequest);
            Assert.IsNotNull(results.Result);


        }

        [TestMethod]
        public void SearchBySKUTestMethod()
        {
            AutoMapperConfig.RegisterMappings();          

            ProductRepository newproduct = new ProductRepository();
            ProductRequest productRequest = new ProductRequest();

            productRequest.PartNumber = "b84-00001";
            productRequest.PoolIds = "52,86";
            productRequest.ProgramOfferings = "ACP,CUS";
            productRequest.SearchPattern = SearchPattern.Exact;
            var results = newproduct.SearchProducts(productRequest);
            Assert.IsNotNull(results.Result);


        }
        [TestMethod]
        public void SearchProductsByPartNumberTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            
            ProductRepository newproduct = new ProductRepository();
            ProductRequest productRequest = new ProductRequest();
            productRequest.PartNumber = "T6A-00024";
           productRequest.ProgramCode = "E6";
           productRequest.AgreementNumber = "5955428";
           productRequest.ProgramOfferings = "CUS";
           productRequest.PurchaseOrderTypeCode = "NE";
           productRequest.LocaleId = 1031;

            var results = newproduct.SearchProductsByPartNumber(productRequest);
            Assert.IsNotNull(results.Result);


        }

        [TestMethod]
        public void SearchProductsTestMethod()
        {
            AutoMapperConfig.RegisterMappings();

            ProductRepository newproduct = new ProductRepository();
            ProductRequest productRequest = new ProductRequest();
            productRequest.PartNumber = null;
            productRequest.AgreementId = 1118461;
            productRequest.ItemName="Office";
            productRequest.ProductFamilyCode = null;
            productRequest.PurchaseOrderTypeCode = "NE";
            DateTime dt= DateTime.Parse("2014-10-27");
            productRequest.UsageDate = dt;
            productRequest.PageSize=100;
            productRequest.PageNumber=1;
            productRequest.SortColumn="ItemName";
            productRequest.IncludeDetails = true;

            var results = newproduct.SearchProducts(productRequest);
            Assert.IsNotNull(results.Result);


        }


        [TestMethod]
        public void GetOpportunitiesByOrderHistoryTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            ProductRepository prod = new ProductRepository();
            var ret = prod.GetOpportunitiesByOrderHistory(1254891, "90758093");
            Assert.IsNotNull(ret.Result);

        }
    }
}
