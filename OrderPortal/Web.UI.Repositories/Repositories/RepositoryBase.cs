using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.Interfaces;
using Web.UI.ServiceGateway.OrderServiceProxy;
namespace Web.UI.Repositories
{
    public class RepositoryBase : IRepository
    {
        private IList<string> _messages = new List<string>();

        public IEnumerable<string> SeriviceMessages
        {
            get
            {
                return _messages;
            }
        }

    }

    
}
