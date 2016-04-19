using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.Data;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.ServiceGateway.OrderServiceProxy;

namespace Web.UI.Repositories.Bridges
{
    public class AgreementServiceBridge : IAgreementBridge
    {

        public async Task<SearchResult<Agreement>> SearchOrderableAgreement(AgreementRequest agreementRequest)
        {
            List<Agreement> agreements = new List<Agreement>();
            SearchResult<Agreement> Output = new SearchResult<Agreement>();
            if (((agreementRequest.AgreementNumber != null || agreementRequest.EndCustomerName != null || agreementRequest.PartnerNumber!=null ||agreementRequest.SalesLocation!=null ||agreementRequest.EndCustomerNumber!=null)&&agreementRequest.LookUpDate!=null))
            {               
                var request = new SearchOrderableAgreementsRequest
                 {
                     RequestId = ConfigData.AgreRequestId,
                     ApplicationId = ConfigData.AgreApplicationId,
                     PageIndex = 0,
                     PageSize = 15,
                     //OrganizationGuid = string.Empty,
                     EndCustomerNumber = agreementRequest.EndCustomerNumber,
                     PartnerPCN = agreementRequest.PartnerNumber,
                     CustomerName = agreementRequest.EndCustomerName,
                     LookupDate =  agreementRequest.LookUpDate,
                     AgreementNumber = agreementRequest.AgreementNumber,
                     SalesLocationCode=agreementRequest.SalesLocation,
                     
                 };

                var client = new OrderServiceClient();
                var response = new SearchOrderableAgreementsResponse();
                response = await client.SearchAgreementsAsync(request);
                agreements.AddRange(Mapper.Map<List<AgreementSummary>, IEnumerable<Agreement>>(response.SearchResults.ToList()));
                Output.Results = agreements;
                Output.TotalCount = agreements.Count();
                return Output;


            }
            else
            {
                return Output;
            }
        }
    }
}
