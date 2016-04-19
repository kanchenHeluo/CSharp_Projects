using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using Web.UI.Repositories;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Test
{
    [Binding]
    public class OrderingSteps
    {
        [Given(@"I have Portal access")]
        public void GivenIHavePortalAccess()
        {
            //ScenarioContext.Current.Pending();
        }

        [Given(@"I have navigated to Orders Page")]
        public void GivenIHaveNavigatedToOrdersPage()
        {
            //ScenarioContext.Current.Pending();
        }

        [When(@"I enter '(.*)' in customer name text box and press Search")]
        public void WhenIEnterInCustomerNameTextBoxAndPressSearch(string orgName)
        {
            //ScenarioContext.Current.Pending();

            AutoMapperConfig.RegisterMappings();
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
            AgreementRequest agrParams = new AgreementRequest();
            //agrParams.AgreementNumber = "6718375";
            agrParams.EndCustomerName = orgName;
            //agrParams.EndCustomerNumber = "50523323";
            agrParams.LookUpDate = dt;
            //agrParams.SalesLocation = "24";
            agrParams.PartnerNumber = null;
            //var output = await agr.SearchOrderableAgreement(agrParams);

            //Assert.IsTrue(output != null && output.TotalCount == 0);

            //Console.WriteLine("Records: {0}", output.TotalCount);

            var outputTask = agr.SearchOrderableAgreement(agrParams);
            outputTask.Wait();

            ScenarioContext.Current.Add("Result", outputTask.Result);
        }

        [Then(@"I should be able to search agreements by agrementnumber, customernumber and customername")]
        public void ThenIShouldBeAbleToSearchAgreementsByAgrementnumberCustomernumberAndCustomername()
        {
            //ScenarioContext.Current.Pending();
        }

        [When(@"I enter '(.*)' in agreement number text box and press Search")]
        public void WhenIEnterInAgreementNumberTextBoxAndPressSearch(string agreementNumber)
        {
            AutoMapperConfig.RegisterMappings();
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
            AgreementRequest agrParams = new AgreementRequest();
            agrParams.AgreementNumber = agreementNumber;
            //agrParams.EndCustomerName = "";
            //agrParams.EndCustomerNumber = "50523323";
            agrParams.LookUpDate = dt;
            //agrParams.SalesLocation = "24";
            agrParams.PartnerNumber = null;
            //var output = await agr.SearchOrderableAgreement(agrParams);

            //Assert.IsTrue(output != null && output.TotalCount == 0);

            //Console.WriteLine("Records: {0}", output.TotalCount);

            var outputTask = agr.SearchOrderableAgreement(agrParams);
            outputTask.Wait();

            ScenarioContext.Current.Add("Result", outputTask.Result);
        }

        [When(@"I enter '(.*)' in customer name text box and '(.*)' in agreementnumber and press Search")]
        public void WhenIEnterInCustomerNameTextBoxAndInAgreementnumberAndPressSearch(string orgName, string agrNum)
        {
            //ScenarioContext.Current.Pending();

            AutoMapperConfig.RegisterMappings();
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
            AgreementRequest agrParams = new AgreementRequest();
            agrParams.AgreementNumber = agrNum;
            agrParams.EndCustomerName = orgName;
            //agrParams.EndCustomerNumber = "50523323";
            agrParams.LookUpDate = dt;
            //agrParams.SalesLocation = "24";
            agrParams.PartnerNumber = null;
            //var output = await agr.SearchOrderableAgreement(agrParams);

            //Assert.IsTrue(output != null && output.TotalCount == 0);

            //Console.WriteLine("Records: {0}", output.TotalCount);

            var outputTask = agr.SearchOrderableAgreement(agrParams);
            outputTask.Wait();

            ScenarioContext.Current.Add("Result", outputTask.Result);
        }


        [Then(@"I should see '(.*)' agreement")]
        public void ThenIShouldSeeAgreement(int count)
        {
            var result = ScenarioContext.Current["Result"] as SearchResult<Agreement>;

            Console.WriteLine("Records: {0}", result.TotalCount);

            Assert.IsTrue(result != null && result.TotalCount == count, "No results found for searched agreement");
        }

        [Then(@"I should see agreements of org '(.*)'")]
        public void ThenIShouldSeeAgreementsOfOrg(string orgName)
        {
            //ScenarioContext.Current.Pending();           
        }

        [Then(@"I should see only agreements from org pcn '(.*)'")]
        public void ThenIShouldSeeOnlyAgreementsFromOrgPcn(string pcn)
        {
            //ScenarioContext.Current.Pending();

            var result = ScenarioContext.Current["Result"] as SearchResult<Agreement>;

            Console.WriteLine("Records: {0}", result.TotalCount);

            Assert.IsTrue(result != null && result.TotalCount > 0, "Search has no results.");

            var hasResult = result.Results.Any(item => item.EndCustomerNumber != pcn);

            //Console.WriteLine("Records: {0}", result.TotalCount);

            Assert.IsFalse(hasResult, "Search has invalid results.");
        }

        [When(@"I search multipe search using customer name agreementnumber")]
        public void WhenISearchMultipeSearchUsingCustomerNameAgreementnumber(Table table)
        {
            ScenarioContext.Current.Pending();

            //foreach(var item in table.Rows)
            //{
            //    //WhenIEnterInCustomerNameTextBoxAndInAgreementnumberAndPressSearch()
            //}
        }


        [Then(@"I should see agreements")]
        public void ThenIShouldSeeAgreements()
        {
            ScenarioContext.Current.Pending();

            //var result = ScenarioContext.Current["Result"] as SearchResult<Agreement>;

            //Assert.IsTrue(result != null && result.TotalCount > 0);

            //Console.WriteLine("Records: {0}", result.TotalCount);
        }

    }
}
