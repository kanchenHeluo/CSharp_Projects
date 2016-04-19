using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    public interface  IDomainObject
    {
         string Name { get; set; }
         string Code { get; set; }
    }
}