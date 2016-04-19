using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Data;
using Web.UI.Repositories.Models;
using System.Threading.Tasks;
using Microsoft.IT.Licensing.Entity.DomainData;
using Microsoft.IT.Licensing.ECDM.DomainData.Services.Client;

namespace Web.UI.Providers
{
    public interface IDomainDataProvider
    {
        Task<ICollection<BillingOption>> GetBillingOptionsAsync(ICollection<string> productTypeCodes, ICollection<string> programCodes, ICollection<string> purchaseOrderTypes, int localeId);
        Task<ICollection<BillingOption>> GetBillingOptionsAsync(string productTypeCode, string programCode, string purchaseOrderType, int localeId);
     List<Web.UI.Repositories.Models.DomainItem> GetSupportedLanguages();
        Task<ICollection<Country>> GetDomainCountryAsync(int localeId);
        Task<ICollection<ProductFamily>> GetProductFamilyAsync(ICollection<string> codes, int localeId);
        Task<ICollection<ProgramOffering>> GetProgramOfferingAsync(ICollection<string> codes, int localeId);
    }
}