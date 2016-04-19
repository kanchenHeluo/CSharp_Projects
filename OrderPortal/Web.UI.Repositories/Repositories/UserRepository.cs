using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.DomainModels;
using Web.UI.ServiceGateway.OrderServiceProxy;

namespace Web.UI.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<UserPreference> GetUserPreferenceAsync(GetUserPreferenceRequest request)
        {
            UserPreference userPre = null;

            var client = new OrderServiceClient();

            var response = await client.GetUserPreferenceAsync(request);

            userPre = new UserPreference
            {
                AddressFormat = response.AddressFormat,
                DateFormat = response.DateFormat,
                Language = response.CultureCode
            };

            return userPre;
        }

        public async Task<bool> SetUserPreferenceAsync(SetUserPreferenceRequest request)
        {
            bool responseStatus = false;

            var client = new OrderServiceClient();
            
            var response = await client.SetUserPreferenceAsync(request);

            responseStatus = response.ResponseStatus;

            return responseStatus;
        }
    }
}
