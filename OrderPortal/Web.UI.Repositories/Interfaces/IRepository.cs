using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Interfaces
{
    public interface IRepository
    {
        /// <summary>
        /// Used to collect user messages while processing service calls so that caller can collect them
        /// </summary>
        IEnumerable<string> SeriviceMessages { get; }
    }
}
