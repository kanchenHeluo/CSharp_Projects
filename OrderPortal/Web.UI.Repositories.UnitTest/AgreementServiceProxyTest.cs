using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.UI.Repositories;
using Web.UI.Repositories.Models;
using System.Collections;
using System.Runtime.Serialization;
using Web.UI.ServiceGateway.OrderServiceProxy;
using Web.UI.Repositories;
using System.Threading.Tasks;
using Web.UI.Repositories.AutoMapper;
using Web.UI.Repositories.DomainModels;
namespace Web.UI.Repositories.UnitTest
{
    [TestClass]
    public class AgreementServiceProxyTest
    {

        [TestMethod]
        public async Task Searchagreements_azureSearchTestMethod()
        {
            //$filter=POAgreementNumber+eq+'6718375'+and+EndCustomerNumber+eq+'50523323'+and+SalesLocationCode+eq+'24'+and+AgreementStartDate+ge+'1/18/2010'
            AutoMapperConfig.RegisterMappings();
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/18/2010");
            AgreementRequest agrParams = new AgreementRequest();
            agrParams.AgreementNumber = "6718375";
            agrParams.EndCustomerName = null;
            agrParams.EndCustomerNumber = "50523323";
            agrParams.LookUpDate = dt;
            agrParams.SalesLocation = "24";
            agrParams.PartnerNumber = null;
            var output = await agr.SearchOrderableAgreement(agrParams);
            Assert.IsNotNull(output);
        }
        [TestMethod]
        public async Task Searchagreements()
        {
            AutoMapperConfig.RegisterMappings();
            AgreementRepository agr=new AgreementRepository();
            DateTime dt = DateTime.Now;
            AgreementRequest agrParams = new AgreementRequest();
            agrParams.AgreementNumber = null;
            agrParams.EndCustomerName = null;
            agrParams.EndCustomerNumber = "02142732";
            agrParams.LookUpDate = DateTime.Now;
            agrParams.SalesLocation = "10";
            agrParams.PartnerNumber = null;
            var output = await agr.SearchOrderableAgreement(agrParams);
            Assert.IsNotNull(output);

            //@AgreementNumber=NULL,@CustomerName=NULL,@UsageDate='2014-12-04 12:27:49.617',@SalesLocationCode=N'10',@EndCustomerNumber=N'02142732',@PartnerPCN=NULL
        }

        [TestMethod]
        public async Task SearchAgreementDetails()
        {
            AutoMapperConfig.RegisterMappings();
            AgreementRepository agr = new AgreementRepository();
            AgreementDetailsRequest agrParams = new AgreementDetailsRequest();
            string[] temp = { "5688706", "5688706" };
            agrParams.AgreementNumbers = temp;
            agrParams.Guid=new Guid("40B115E2-DB8B-491F-AB91-F92CC13D2BD1");
            var output = await agr.GetAgreementDetails(agrParams);
            Assert.IsNotNull(output);
        }


        [TestMethod]
        public async Task GetCustomersDetails()
        {
            AutoMapperConfig.RegisterMappings();
            AgreementRepository agr = new AgreementRepository();
            AgreementRequest request = new AgreementRequest();
            request.PartnerNumber = "A0458A41";
            request.LookUpDate = DateTime.Parse("2014/11/11");
            var output = await agr.GetCustomers(request);
            Assert.IsNotNull(output);
        
        }
    }
}
