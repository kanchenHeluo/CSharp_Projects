using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;
using Web.UI.ServiceGateway.OrderServiceProxy;

namespace Web.UI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserPreference> GetUserPreferenceAsync(GetUserPreferenceRequest request);
        Task<bool> SetUserPreferenceAsync(SetUserPreferenceRequest request);
    }
}
