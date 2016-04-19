using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.OrderServiceProxy;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Repositories.Interfaces
{
    public interface IAgreementRepository : IRepository,IAgreementBridge
    {

       
        /// <summary>
        /// Getting Agreements Results in Detail From Order Service
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="agreementnumbers"></param>
        /// <returns></returns>
        Task<SearchResult<Agreement>> GetAgreementDetails(AgreementDetailsRequest agrDetailPrams);

        Task<IEnumerable<Customer>> GetCustomers(AgreementRequest request);

    }
}