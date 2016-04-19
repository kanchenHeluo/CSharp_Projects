using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Repositories.Interfaces
{
    public interface IAgreementBridge
    {
        /// <summary>
        ///Getting Agreements from Order Service
        ///</summary>
        /// <param name="partnerNumber"></param>
        /// <param name="endCustomerNumber"></param>
        /// <param name="agreementNumber"></param>
        /// <param name="endCustomerName"></param>
        /// <param name="lookUpDate"></param>
        /// <returns></returns>
        Task<SearchResult<Agreement>> SearchOrderableAgreement(AgreementRequest agrParams);
    }
}
