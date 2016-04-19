using Newtonsoft.Json.Linq;
using System;

namespace CRMRestService.Interface
{
    public interface IRepositry
    {
        void Execute(JObject input);
        Guid AssociateAccountId { get; set; }
    }
}
