using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Web.UI.Repositories;
using Web.UI.Repositories;
using Web.UI.Repositories.AutoMapper;
using Web.UI.Repositories.Bridges;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.OrderServiceProxy;
namespace Web.UI.Repositories.UnitTest
{



    [TestClass]
    public class OrderRepositoryTest
    {

        [TestMethod]
        public void OrderRepositoryGetDraftOrderTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            int? draftOrderId = null;
            string pcnFilter = "91074423"; 
            string userName = "fareast\ronli";
            var ret = ord.GetDraftOrder(draftOrderId, pcnFilter, userName);
            ret.Wait();
            var result = ret.Result;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void OrderRepositoryServiceBridgeTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            DateTime dt = DateTime.Now;
            OpportunityRequest param = new OpportunityRequest();
          //  ord.OrderBridge.GetNonResStepUpOpportunties = new OrderServiceBridge;
            param.AgreementNumber = "70757828";
            param.EndCustomerNumber = "BF80C800";
            param.PublicCustomerNumber = "8484";
            param.POAgreementNumber = "S9067344";
            param.PageNumber = 1;
            param.PageSize = 1;
            param.SortColumn = "LineItemId";
            var output =  ord.GetNonResStepUpOpportunties(param);
            Assert.IsNotNull(output.Result);
  
        }

        [TestMethod]
        public void OrderRepositoryRenewalServiceBridgeTestMethod()
        {
            
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            DateTime dt = DateTime.Now;
            OpportunityRequest param = new OpportunityRequest();
            param.AgreementNumber = "70757828";
            param.EndCustomerNumber = "BF80C800";
            param.PublicCustomerNumber = "8484";
            param.POAgreementNumber = "S9067344";
            param.PageNumber = 1;
            param.PageSize = 1;
            param.SortColumn = "LineItemId";
            var output = ord.GetRenewalOpportunties(param);
            Assert.IsNotNull(output.Result);
            }


        [TestMethod]
        public void OrderRepositoryAzureBridgeTestMethod()
        {

            
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            DateTime dt = DateTime.Now;
            OpportunityRequest param = new OpportunityRequest();
            param.AgreementNumber = "46306916";
            param.EndCustomerNumber = "B97B1594";
            param.PublicCustomerNumber = "8484";
            param.POAgreementNumber = "46306916";
            var output = ord.GetNonResStepUpOpportunties(param);
            Assert.IsNotNull(output.Result);

        }

        [TestMethod]
        public void OrderRepositoryRenewalAzureBridgeTestMethod()
        {
            
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            DateTime dt = DateTime.Now;
            OpportunityRequest param = new OpportunityRequest();
            param.AgreementNumber = "47693882";
            param.EndCustomerNumber = "B97523A5";
            param.PublicCustomerNumber = "8484";
            param.POAgreementNumber = "S9041811";
            var output = ord.GetRenewalOpportunties(param);
            Assert.IsNotNull(output.Result);
        }
        [TestMethod]
        public void OrderRepositoryTrueUpTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            DateTime dt = DateTime.Now;
            OpportunityRequest param = new OpportunityRequest();        
            param.AgreementNumber = "5295724";
            param.EndCustomerNumber = "69756243";           
            param.POAgreementNumber = "5295724";
            param.PublicCustomerNumber = "8484";
            var output = ord.GetTrueUpOpportunities(param);
            Assert.IsNotNull(output.Result);
        }

        [TestMethod]
        public void OrderRepositoryGetLineItemAttributesTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
           
            LineItemAttributeRequest lineItemRequest = new LineItemAttributeRequest() { 
            ItemId="1403676",
             UsageDate=Convert.ToDateTime("10/26/2014"),
            AgreementId = 5352779,
            ProgramCode="E6",LocaleId=1033
            };
            var output = ord.GetLineItemAttributes(lineItemRequest);
            Assert.IsNotNull(output.Result);
        }
        [TestMethod]
        public void OrderRepositoryGetOrderHeaderAttributesTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            int localeid = 5353448;
            var output = ord.GetOrderHeaderAttributes(localeid);
            Assert.IsNotNull(output.Result);
        }

        [TestMethod]
        public void OrderRepositoryCreateOrder()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var settings = configFile.AppSettings.Settings;
            var surfix = settings["CurrentPOSurfix"].Value;
            var nextSurfix = (int.Parse(surfix) + 1).ToString();
            settings["CurrentPOSurfix"].Value = nextSurfix;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("AppSettings");
            OrderHeader order = new OrderHeader()
            {
                AgreementId = 5354158,
                AgreementNumber = "4613234",
                EndCustomerNumber = "43465623",
                DirectCustomerNumber = "43465623",


               OrderName = "Tiger's abc",
               PricingCurrencyCode = "USD",
               PricingCountryCode = "US",
               ProgramName = "E6",
               PurchaseOrderDate = DateTime.Now,
               SalesLocationCode = "10",
                UsageDate =  DateTime.Now,
                CreatedUser = @"fareast\tigeren",
                ModifiedUser = @"fareast\tigeren",
                //PurchaseOrderType = "NE",
                PurchaseOrderNumber = "4613234_tiger_PO_" + surfix,
                SourceCode = "MAN",
                ProgramCode = "E6"
            };
            //Configuration
            OrderLineItem lineItem = new OrderLineItem()
            {
                ProductTypeCode = "MSU",
                UsageCountryCode = "US",
                PartNumber = "BT1-00019",
                POLIUsageDateTime = DateTime.Now,
                OrderQuantity = 100,
                ItemId = 1228684,
                CoverageStartDate = new DateTime(2014,11,01),
                CoverageEndDate = new DateTime(2015,10,31),
                BillingOption = "AE",
                LineItemType = "NEW", 
                IsOlsItem = true,
            };

            OrderRepository orderRepository = new OrderRepository();
            var createTask = orderRepository.CreateOrder(order, new List<OrderLineItem>() {lineItem},null, @"fareast\tigeren");
            createTask.Wait();
            var response =  createTask.Result.ToList();
            Assert.AreEqual(1,response.Count);
            Assert.AreEqual(true, response.First().Order.LineItems.Any());

        }

        [TestMethod]
        public void OrderRepositoryBuildCreateVLOrderRequest()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            OrderHeader order = new OrderHeader()
            {
                AgreementId = 5354158,
                AgreementNumber = "4613234",
                EndCustomerNumber = "43465623",
                DirectCustomerNumber = "43465623",
                OrderName = "Tiger's abc",
                PricingCurrencyCode = "USD",
                PricingCountryCode = "US",
                ProgramName = "E6",
                PurchaseOrderDate = DateTime.Now,
                SalesLocationCode = "10",
                UsageDate = DateTime.Now,
                CreatedUser = @"fareast\tigeren",
                ModifiedUser = @"fareast\tigeren",
                //PurchaseOrderType = "NE",
                PurchaseOrderNumber = "4613234_tiger12",
                SourceCode = "MAN",
                ProgramCode = "E6"
            };

            DateTime startDateTime, endDateTime;
            
            OrderLineItem lineItem = new OrderLineItem()
            {
                ProductTypeCode = "MSU",
                UsageCountryCode = "US",
                PartNumber = "BT1-00019",
                POLIUsageDateTime = DateTime.Now,
                OrderQuantity = 100,
                ItemId = 1228684,
                CoverageStartDate = new DateTime(2014, 11, 01),
                CoverageEndDate = new DateTime(2015, 10, 31)

            };
            Agreement agreement = new Agreement()
            {
                AgreementId = 5354158,
                AgreementNumber = "4613234",
                CurrencyCode = "USD",
                EndCustomerNumber = "43465623",
                ProgramCode = "E6",
                
            };
            //OrderRepository.OAPCoverageDateCalculation(agreement.ProgramCode, lineItem.POLIUsageDateTime.GetValueOrDefault(),null, null, agreement.EndEffectiveDate.GetValueOrDefault(),"12",lineItem)
            var repository = new OrderRepository();
            var result = repository.BuildCreateVlOrderRequest(order, new List<OrderLineItem>() {lineItem}, new Audit()
            {
                CreatedAt = DateTime.Now,
                CreatedBy = "fareast\\tigeren",
                ModifiedAt = DateTime.Now,
                ModifiedBy = "fareast\\tigeren"
            });
            Assert.AreEqual("bt1-00019", result.Order.LineItems[0].ItemDetails.Number);
        }

        [TestMethod]
        public void OrderRepositoryGetOrderEstimate()
        {
            AutoMapperConfig.RegisterMappings();
            Agreement agreement = new Agreement()
            {
                AgreementId = 5354158,
                AgreementNumber = "4613234",
                CurrencyCode = "USD",
                EndCustomerNumber = "43465623",
                ProgramCode = "E6",

            };
            OrderHeader order = new OrderHeader()
            {
                AgreementId = 5354158,
                AgreementNumber = "4613234",
                EndCustomerNumber = "43465623",
                DirectCustomerNumber = "43465623",
                OrderName = "Tiger's abc",
                PricingCurrencyCode = "USD",
                PricingCountryCode = "US",
                ProgramName = "E6",
                PurchaseOrderDate = DateTime.Now,
                SalesLocationCode = "10",
                UsageDate = DateTime.Now,
                CreatedUser = @"fareast\tigeren",
                ModifiedUser = @"fareast\tigeren",
                //PurchaseOrderType = "NE",
                SourceCode = "MAN",
                ProgramCode = "E6"
            };
            //Configuration
            OrderLineItem lineItem = new OrderLineItem()
            {
                ProductTypeCode = "MSU",
                UsageCountryCode = "US",
                PartNumber = "BT1-00019",
                POLIUsageDateTime = DateTime.Now,
                OrderQuantity = 100,
                ItemId = 1228684,
                CoverageStartDate = new DateTime(2014, 11, 01),
                CoverageEndDate = new DateTime(2015, 10, 31),
                BillingOption = "AE",
//                LineItemType = "NEW",
                IsOlsItem = true,
            };
            var orderRepository = new OrderRepository();
            var estimateTask = orderRepository.GetPriceEstimate(lineItem, agreement, "NE");
            estimateTask.Wait();
            var estimate = estimateTask.Result;
            Assert.AreEqual("3.96",estimate.ListPrice.ToString());
        }

        [TestMethod]
        public void OrderRepositorySaveDraftOrderTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            
            OrderRepository ord = new OrderRepository();
            OrderHeader orderHeader = new OrderHeader()
            {
                OrderName = DateTime.Now.ToString(),
                AgreementId = 5096218,
                AgreementNumber = "8215470",
                SalesLocationCode = "IE",
                ProgramCode = "CC",
                PricingCountryCode = "EDU",
                PricingCurrencyCode = "EUZ",
                EndCustomerNumber = "B3960C56",
                DirectCustomerNumber = "0005003261",
                PurchaseOrderDate = Convert.ToDateTime("2013-12-16 00:00:00"),
                UsageDate = Convert.ToDateTime("2013-12-16 00:00:00"),
                PurchaseOrderTypeCode = "NE",
                PurchaseOrderNumber = DateTime.Now.ToString(),
                ValidateFlag = false,
                LockedBy = null,
                LockedFlag = false
            };

            List<OrderLineItem> lineitems = new List<OrderLineItem>();
            OrderLineItem lineitem = new OrderLineItem()
            {
                OrderQuantity = 20,
                QuantityOrdered = 10,
                UsageCountryCode = "US",
                PartNumber = "bt1-00019",
                POLIUsageDateTime = Convert.ToDateTime("2013-12-16 00:00:00"),
                ProgramOfferingCode = "ACP",
                UnitPrice = Convert.ToDecimal(1),
                PurchaseUnitTypeCode = "12",
                PurchaseUnitQuantity = "12",
                PricingDrivers = new PricingDrivers()
                {
                    PricingCountryCode = "EDU",
                    PricingCurrencyCode = "EUZ"
                },
                BillingOption = "AE",
                ItemName = "OfficeLiveMtgStd ShrdSvr ALNG SubsVL MVL PerUsr",
             
                ItemId = 1228684,
                IsPriced = true,
                PoolCode = "A",
                CoverageTerm = 1,
                CoverageStartDate = Convert.ToDateTime("2013-12-16 00:00:00"),
                CoverageEndDate = Convert.ToDateTime("2014-12-16 00:00:00"),
                LineItemType = "OAP",
                ValidateFlag = false
            };
            lineitems.Add(lineitem);

            var output = ord.SaveDraftOrder(orderHeader, lineitems, "kanchen", Guid.NewGuid());
            Assert.IsNotNull(output.Result);
        }
        [TestMethod]
        public void GetPriceEstimateTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            
            OrderRepository ord = new OrderRepository();
            OrderLineItem lineitem = new OrderLineItem()
            {
                OrderQuantity = 20,
                QuantityOrdered = 10,
                UsageCountryCode = "US",
                PartNumber = "UT6-00005",
                POLIUsageDateTime = Convert.ToDateTime("2015-2-1  12:00:00"),
                ProgramOfferingCode = "CUS",
                UnitPrice = Convert.ToDecimal(1),
                PurchaseUnitTypeCode = "12",//
                PurchaseUnitQuantity = "12",//
                PricingDrivers = new PricingDrivers()
                {
                    PricingCountryCode = "US",//
                    PricingCurrencyCode = "USD"//
                },
                BillingOption = "AE",
                ItemName = "OfficeLiveMtgStd ShrdSvr ALNG SubsVL MVL PerUsr",
                ItemId = 1228684,//
                IsPriced = true,//
                PoolCode = "A",//
                CoverageTerm = 1,
                CoverageStartDate = Convert.ToDateTime("2015-2-1  12:00:00"),
                CoverageEndDate = Convert.ToDateTime("2016-1-31  11:59:00"),
                LineItemType = "TGT",
                ValidateFlag = false,
                LineItemGuid = Guid.NewGuid()
                
            };
            Web.UI.Repositories.DomainModels.Agreement  agreementheader= new Agreement();
            agreementheader.AgreementId = 4540041;
            agreementheader.AgreementNumber = "50141157";
            agreementheader.EndCustomerNumber = "A93FEFD5";
            var output = ord.GetPriceEstimate(lineitem,agreementheader,"NE");
            Assert.IsNotNull(output.Result);

        }

        [TestMethod]
        public void GetSalesLocationTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            
            OrderRepository ord = new OrderRepository();
            var output = ord.GetSalesLocations();
            Assert.IsNotNull(output.Result);
        }


        [TestMethod]
        public void OAPCalculateCoverageDateTestUnitQuantity12Method()
        {
            var ret = OrderRepository.OAPCoverageDateCalculation("E6", DateTime.Now, null, null, DateTime.MinValue, "12", "AE");
            Assert.IsNotNull(ret.Key);
            Assert.IsNotNull(ret.Value);
        }

        [TestMethod]
        public void OAPCalculateCoverageDateTestMethod()
        {
            var ret = OrderRepository.OAPCoverageDateCalculation("E6", DateTime.Now, null, null, new DateTime(2015,11,30), "-1", "AE");
            Assert.IsNotNull(ret.Key);
            Assert.IsNotNull(ret.Value);
        }
        [TestMethod]
        public void OAPCalculateCoverageDateTestUnitQuantityEmptyMethod()
        {
            var ret = OrderRepository.OAPCoverageDateCalculation("E6", DateTime.Now, null, null, DateTime.MinValue, null, "AE");
            Assert.IsNotNull(ret.Key);
            Assert.IsNotNull(ret.Value);
        }

        [TestMethod]
        public void SearchOrdersTestMethod()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
           
            string[] codeTable = new string[1];
            codeTable[0] = "HAD";
            var ret = ord.GetOrdersWithStatus("35930142", "", 0, "", 1, 50, codeTable);
            Assert.IsNotNull(ret);
        }

        [TestMethod]
        public async Task SaveShipmentAddressResultTest()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository or = new OrderRepository();
            var shipment = new Shipment
            {
                PurchaseOrderShipToId = 699999,
                AgreementID = 699999,
                AgreementNumber = "3333",
                LicenseProgramCode = "uuu",
                OrganizationName = "org",
                AddressLine1 = "address1",
                CountryCode = "ch",
                ContactEmailAddress = "1@1.com",
                City = "beijigng",
                ContactPhoneNumber = "12222",
                ContactFirstName = "paul"
            };

            var userName = "testShipUser";

            var response = await or.SaveShipmentAddress(shipment, userName);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Errors != null && response.Errors.Any());
            Assert.IsNotNull(response.ShipToId);
        }

        [TestMethod]
        public void OrderRepositoryDeleteDraftOrder()
        {
            AutoMapperConfig.RegisterMappings();
            OrderRepository ord = new OrderRepository();
            var result = ord.DeleteDraftOrder(21466, @"fareast\tigeren", 2);
        }
    }
}
