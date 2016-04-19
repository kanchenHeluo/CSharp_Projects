using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway;
using Web.UI.ServiceGateway.OrderServiceProxy;
using Web.UI.Repositories.Data;
using System.Collections.ObjectModel;
using Web.Common.Extensions;
using AutoMapper;
using Web.UI.Repositories.AutoMapper;
using Web.UI;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Bridges;

namespace Web.UI.Repositories
{
    public class AgreementRepository : RepositoryBase, IAgreementRepository
    {

        public AgreementRepository()
        {
            AgreementBridge = new AgreementServiceBridge();
        }
        
    
        public IAgreementBridge AgreementBridge { get; set; }

        public async Task<SearchResult<Agreement>> SearchOrderableAgreement(AgreementRequest agreementRequest)
        {

           return await AgreementBridge.SearchOrderableAgreement(agreementRequest);
        }

        public async Task<SearchResult<Agreement>> GetAgreementDetails(AgreementDetailsRequest agreementRequest)
        {
            List<Agreement> agreements = new List<Agreement>();
            SearchResult<Agreement> Output = new SearchResult<Agreement>();
            if (agreementRequest.Guid != null)
            {
                    var request = new GetOrderableAgreementDetailsRequest
                    {
                        RequestId = ConfigData.AgreRequestId,
                        ApplicationId = ConfigData.AgreApplicationId,
                        AgreementNumbers = agreementRequest.AgreementNumbers
                    };
                    var client = new OrderServiceClient();
                    var response = new GetOrderableAgreementDetailsResponse();
                    response = await client.GetOrderableAgreementDetailsAsync(request);
                    agreements.AddRange(Mapper.Map<List<AgreementDetail>, IEnumerable<Agreement>>(response.AgreementDetails.ToList()));
                    Output.Results = agreements;
                    Output.TotalCount = agreements.Count();
                    return Output;
                
            }
            else
            {
                return Output;
            }

        }

        public async Task<IEnumerable<Customer>> GetCustomers(AgreementRequest request)
        {
            
             var client = new OrderServiceClient();
             List<GetCustomerResponse> response = new List<GetCustomerResponse>();
            GetCustomerRequest req =new GetCustomerRequest();
            req.PartnerPCN=request.PartnerNumber;
            req.UsageDate=request.LookUpDate;
           // var agreements = client.GetCustomersAsync(req);
            var resp=await client.GetCustomersAsync(req);
            response.AddRange(resp);
            var Cust = response.ToList().Select(i => new Customer { EndCustomerName = i.CutomerName, EndCustomerNumber = i.CustomerPCN });  
            return  Cust;
        }

    }
}
    
      
    

