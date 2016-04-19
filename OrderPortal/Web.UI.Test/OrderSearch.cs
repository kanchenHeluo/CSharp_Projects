using fit;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Web.UI.Repositories;
using Web.UI.Repositories.AutoMapper;
using Web.UI.Repositories.Bridges;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Models;
namespace Web.UI.Test.Fixtures
{
    public abstract class OrderSearchBaseFixture : ColumnFixture
    {
        public string Agreement_Number { get; set; }

        public string EndCustomer_Number { get; set; }
        public string DirectCustomer_Number { get; set; }
        public string ProductType_Code { get; set; }
        public string Part_Number { get; set; }
        public int Item_Id { get; set; }
        public string Program_Code { get; set; }
        public int Agreement_Id { get; set; }
        public int? draftOrderId { get; set; }
        protected OrderSearchBaseFixture()
        {
           
            AutoMapperConfig.RegisterMappings();
        }
    }



    public class CreateOrder : OrderSearchBaseFixture
    {
         public int HasResult()
        {
           var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
          
            var settings = configFile.AppSettings.Settings;
            var surfix = settings["CurrentPOSurfix"].Value;
            var nextSurfix = (int.Parse(surfix) + 1).ToString();
            settings["CurrentPOSurfix"].Value = nextSurfix;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("AppSettings");

            OrderHeader order = new OrderHeader()
            {
                AgreementId = Agreement_Id,//5354158,
                AgreementNumber = Agreement_Number,//"4613234",
                EndCustomerNumber = EndCustomer_Number,//"43465623",
                DirectCustomerNumber = DirectCustomer_Number,//"43465623",
                OrderName = "Tiger's abc",
                PricingCurrencyCode = "USD",
                PricingCountryCode = "US",
                ProgramName = Program_Code,
                PurchaseOrderDate = DateTime.Now,
                SalesLocationCode = "10",
                UsageDate = DateTime.Now,
                CreatedUser = @"fareast\tigeren",
                ModifiedUser = @"fareast\tigeren",
                //PurchaseOrderType = "NE",
                PurchaseOrderNumber = "4613234_tiger_PO_" + 1,
                SourceCode = "MAN",
                ProgramCode = Program_Code
            };
            //Configuration
            OrderLineItem lineItem = new OrderLineItem()
            {
                ProductTypeCode = ProductType_Code,//"MSU",
                UsageCountryCode = "US",
                PartNumber = Part_Number,//"BT1-00019",
                POLIUsageDateTime = DateTime.Now,
                OrderQuantity = 100,
                ItemId = Item_Id,//1228684,
                CoverageStartDate = new DateTime(2014, 11, 01),
                CoverageEndDate = new DateTime(2015, 10, 31),
                BillingOption = "AE",
                LineItemType = "NEW",
                IsOlsItem = true,
            };
            OrderRepository orderRepository = new OrderRepository();
            var outputTask = orderRepository.CreateOrder(order, new List<OrderLineItem>() { lineItem }, null, @"fareast\tigeren");
            outputTask.Wait();
            var response = outputTask.Result.ToList();
            return response.Count;



        }

    }


    public class GetDraftOrder : OrderSearchBaseFixture
    {

        public int HasResult()
        {
            OrderRepository orderRepository = new OrderRepository();
            var outputTask = orderRepository.GetDraftOrder(draftOrderId, EndCustomer_Number, "fareast\ronli");
            outputTask.Wait();
            var response = outputTask.Result;
            return outputTask.Result.Count();
        }
    }
    public class DeleteDraftOrder : OrderSearchBaseFixture
    {
        public long draftOrderId { get; set; }
        public string username { get; set; }
        public long Result()
        {
            OrderRepository orderRepository = new OrderRepository();
            var outputTask = orderRepository.DeleteDraftOrder(draftOrderId, username,1);
            outputTask.Wait();
            var response = outputTask.Result;
            return outputTask.Result;
        }
    }
}
