using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.UI;
using Web.UI.Controllers;
using Web.Common;
using Web.UI.Repositories;
using Web.UI.ServiceGateway;
using Web.UI.UnitOfWork;
using Microsoft.Practices.Unity;


using Web.UI.Repositories.DomainModels;
using System.Collections.Generic;
using Web.UI.Providers;

using Web.UI.Models;
using System.Linq;
using System.Web.Mvc;

namespace Web.UI.Test
{
    [TestClass]
    public class DomainDataControllerTest
    {
        [Dependency]
        public IPortalUnitOfWork PortalUnitOfWork { get; set; }
        [TestInitialize]
        public void TestSetup()
        {

        }


        [TestMethod]
        public void OrderHeaderAttributesTestMethod()
        {
            AutoMapperConfig.RegisterMappings();

            DomainDataController domainCtrl = new DomainDataController();
            domainCtrl.PortalUnitOfWork = new PortalUnitOfWork() {OrderRepository = new OrderRepository()};
            var temp2 = domainCtrl.OrderHeaderAttributes(5353448,DateTime.Now);
            
            //var temp = new dom(5375994, DateTime.Now, "2");
          //  Assert.IsNotNull(temp2);

        }
        [TestMethod]
        public void LineAttributesTestMethod()
        {
            AutoMapperConfig.RegisterMappings();

            DomainDataController domainCtrl = new DomainDataController();
            domainCtrl.PortalUnitOfWork = new PortalUnitOfWork() {OrderRepository = new OrderRepository()};
            EcdmDomainProvider ecdmProvider = new EcdmDomainProvider();
            ecdmProvider.cache = new InMemoryCacheProvider();
            LineItemRequest request = new LineItemRequest();
            var productTypeCode = new List<string> { "LSA" };
            var programCode = new List<string> { "S3" };
            var purchaseOrderType = new List<string> { "NCP" };
            var prodFamilyCodes = new List<string> { "STD", "FAC" };          
            request.ProductTypeCodes = productTypeCode;
            request.ProgramCodes = programCode;
            request.PurchaseOrderTypes = purchaseOrderType;
            request.LocaleId = 1031;
            request.ProductFamilyCodes = prodFamilyCodes;
            domainCtrl.DomainData = ecdmProvider;
            LineItemAttributes lineitems = new LineItemAttributes();
            var lineAttributesResult = domainCtrl.LineItemAttributes(request);
            Assert.IsNotNull(lineAttributesResult.Result.Data);
           // object lineitem = ((JsonResult)lineAttributesResult.Result.Data);
         
           //lineitems = (LineItemAttributes)((JsonResult)lineitem).Data;
          
           //Assert.IsTrue(lineitems.BillingOptions != null && lineitems.BillingOptions.Any(),"Billing Option is not valid");
           //Assert.IsTrue(lineitems.ProgramOfferings != null && lineitems.ProgramOfferings.Any(), "Program Offering  is not valid");
          
        }

        [TestMethod]
        public void GetLanguagesTestMethod()
        {
            AutoMapperConfig.RegisterMappings();

            DomainDataController domainCtrl = new DomainDataController();
            domainCtrl.PortalUnitOfWork = new PortalUnitOfWork() {OrderRepository = new OrderRepository()};
            EcdmDomainProvider ecdmProvider = new EcdmDomainProvider();
            ecdmProvider.cache = new InMemoryCacheProvider();
            domainCtrl.DomainData = ecdmProvider;
            var languages = domainCtrl.GetLanguages();
            Assert.IsNotNull(languages);
           

        }
  
    }
}
