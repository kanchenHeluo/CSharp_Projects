using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using Web.UI.Data;
using Web.UI.Repositories.Models;
using System.Web.Mvc;
using Microsoft.IT.Licensing;
using Microsoft.IT.Licensing.ECDM.DomainData.Services.Client;
using Microsoft.IT.Licensing.Entity.DomainData;
using System.Data.Services.Client;
using System.Linq.Expressions;
using System.Web.Caching;
using Microsoft.IT.Licensing.DomainDataClient;
using System.Threading.Tasks;
using System.Collections;
using Telerik.Web.Mvc.Extensions;
using Web.UI.Common;
//using Web.UI.Repositories;

namespace Web.UI.Providers
{
    public class EcdmDomainProviderMock : IDomainDataProvider
    {
        public Task<ICollection<BillingOption>> GetBillingOptionsAsync(ICollection<string> productTypeCodes, ICollection<string> programCodes, ICollection<string> purchaseOrderTypes, int localeId)
        {
            var ta =  new Task<ICollection<BillingOption>>(() => new List<BillingOption>() { 
                 new BillingOption(){Name="Annual Billing", Code="AE"},
                 new BillingOption(){Name="Monthly Billing", Code="ME"}
            });
            ta.Start();
            return ta;
        }

        /// <summary>
        /// Get Billing Option
        /// </summary>
        public Task<ICollection<BillingOption>> GetBillingOptionsAsync(string productTypeCode, string programCode, string purchaseOrderType, int localeId)
        {
            var ta = new Task<ICollection<BillingOption>>(() => new List<BillingOption>() { 
                 new BillingOption(){Name="Annual Billing", Code="AE"},
                 new BillingOption(){Name="Monthly Billing", Code="ME"}
            });
            ta.Start();
            return ta;
        }

        /// <summary>
        /// Get Product Family
        /// </summary>

        public Task<ICollection<ProductFamily>> GetProductFamilyAsync(ICollection<string> codes, int localeId)
        {
            var ta = new Task<ICollection<ProductFamily>>(() => new List<ProductFamily>() { 
                 new ProductFamily(){Name="LPT", Code="LPT"}
            });
            ta.Start();
            return ta;
        }
        /// <summary>
        /// Get Program Offering
        /// </summary>
        public Task<ICollection<ProgramOffering>> GetProgramOfferingAsync(ICollection<string> codes, int localeId)
        {
            var ta = new Task<ICollection<ProgramOffering>>(() => new List<ProgramOffering>() { 
                 new ProgramOffering(){Name="ES Enterprise", ProgramCode= "CUS", Code = "LPT"},
                 new ProgramOffering(){Name="Campus 1 Student", ProgramCode="STU", Code = "LPT"}
            });
            ta.Start();
            return ta;
        }

        /// <summary>
        /// Get Domain Country
        /// </summary>
        public Task<ICollection<Country>> GetDomainCountryAsync(int localeId)
        {
            var ta = new Task<ICollection<Country>>(() => new List<Country>() { 
                 new Country(){Name="China", Code="CN"},
                 new Country(){Name="India", Code="IN"},
                 new Country(){Name="United States", Code="US"}
            });
            ta.Start();
            return ta;
        }

        /// <summary>
        /// Get PSS Status
        /// </summary>

        /// <summary>
        /// Get Locale
        /// </summary>
        public List<Web.UI.Repositories.Models.DomainItem> GetSupportedLanguages()
        {
            return new Web.UI.Repositories.Models.DomainItem[]
            {
             new Web.UI.Repositories.Models.DomainItem()   {Code= "en-us",Category= "en"}
            }.ToList();
        }
    }
}