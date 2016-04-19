using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Models;

namespace Web.UI.Repositories.Interfaces
{
    public interface IProductRepository: IRepository
    {

        /// <summary>
        /// Returns products that are due for renewals/extensions
        /// </summary>
        /// <param name="partnerPCN"> Unique aplhanumeric value of partner participant ie. DIR,SOA participant types </param>
        /// <param name="customerPCN">Unique aplhanumeric value of end customer ie: PRI,AFF participant types</param>
        /// <param name="agreementNumber">Unique aplhanumeric value of a contract</param>
        /// <returns></returns>
        Task<IEnumerable<OderableItem>> SearchRenewalProducts(string partnerPCN, string customerPCN, string agreementNumber);
        /// <summary>
        /// Returns products that are eligible for stepup/updgrade
        /// </summary>
        /// <param name="partnerPCN"> Unique aplhanumeric value of partner participant ie. DIR,SOA participant types </param>
        /// <param name="customerPCN">Unique aplhanumeric value of end customer ie: PRI,AFF participant types</param>
        /// <param name="agreementNumber">Unique aplhanumeric value of a contract</param>
        /// <returns></returns>
        Task<IEnumerable<OderableItem>> SearchStepupProducts(string partnerPCN, string customerPCN, string agreementNumber);
        /// <summary>
        /// Returns any products  that are eligible for supplied partner ,customer and agreement number combination.
        /// </summary>
        /// <param name="partnerPCN"> Unique aplhanumeric value of partner participant ie. DIR,SOA participant types </param>
        /// <param name="customerPCN">Unique aplhanumeric value of end customer ie: PRI,AFF participant types</param>
        /// <param name="agreementNumber">Unique aplhanumeric value of a contract</param>
        /// <returns></returns>
        IEnumerable<OderableItem> SearchProducts(string partnerPCN, string customerPCN, string agreementNumber);
        /// <summary>
        /// Returns products  based on prior purchase history and special deals
        /// </summary>
        /// <param name="partnerPCN"> Unique aplhanumeric value of partner participant ie. DIR,SOA participant types </param>
        /// <param name="customerPCN">Unique aplhanumeric value of end customer ie: PRI,AFF participant types</param>
        /// <param name="agreementNumber">Unique aplhanumeric value of a contract</param>
        /// <returns></returns>
        IEnumerable<OderableItem> SearchCommitedProducts(string partnerPCN, string customerPCN, string agreementNumber);

      Task<IEnumerable<AvailableInPool>> SearchAvialablePool(string itemId);
      Task<SearchResult<OrderLineItem>> SearchProducts(ProductRequest productRequest);
      Task<IEnumerable<AgreementRef>> SearchAgreement(string partnerPCN, string customerPCN, string customerName, string agreementNumber, string UsageDate);
      Task<IEnumerable<LicensePool>> SearchLicense(string licensePoolId);
      Task<IEnumerable<OrderLineItem>> SearchProductsByPartNumber(ProductRequest productRequest);

      /// <summary>
      /// Get Purchase Orders in Detail 
      /// </summary>
      /// <param name="agreementId"></param>
      /// <param name="endCustomerNumber"></param>
      /// <returns></returns>
      Task<IEnumerable<OrderLineItem>> GetOpportunitiesByOrderHistory(int agreementId, string endCustomerNumber);
    }
}