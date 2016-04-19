using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.UI.DomainDataRepository;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IT.Licensing.Entity.DomainData;
using Web.UI;
using Web.UI.Providers;
using Web.UI.Cache;
namespace Web.UI.DomainDataRepository.UnitTest
{
    [TestClass]
    public class DomainDataTest
    {
        [TestMethod]
        public async Task GetBillingOptionsTestMethod()
        {
            //  ICacheProvider cache ;
            //cache.
            EcdmDomainProvider domainData = new EcdmDomainProvider();
            domainData.cache = new InMemoryCacheProvider();
            var billingData = await domainData.GetBillingOptionsAsync("LAV","OLV","NE",1031);
            Assert.IsNotNull(billingData);
        }

        [TestMethod]
        public async Task GetProductFamilyTestMethod()
        {
            EcdmDomainProvider domainData = new EcdmDomainProvider();
            domainData.cache = new InMemoryCacheProvider();
            int locale = 1043;
            var prodFamilyCodes = new List<string> { "STD", "FAC" };
            var ProductFamily = await domainData.GetProductFamilyAsync(prodFamilyCodes, locale);
            Assert.IsNotNull(ProductFamily);
        }


        [TestMethod]
        public async Task GetProgramOfferingTestMethod()
        {
            EcdmDomainProvider domainData = new EcdmDomainProvider();
            domainData.cache = new InMemoryCacheProvider();
            int locale = 1043;
            var prodFamilyCodes = new List<string> { "STD", "FAC" };
            var programOffering = await domainData.GetProgramOfferingAsync(prodFamilyCodes, locale);
            Assert.IsNotNull(programOffering);
        }

        [TestMethod]
        public async Task GetDomainCountryTestMethod()
        {
            EcdmDomainProvider domainData = new EcdmDomainProvider();
            domainData.cache = new InMemoryCacheProvider();
            int locale = 1043;
            var country = await domainData.GetDomainCountryAsync(locale);
            Assert.IsNotNull(country);
        }
    }
}
