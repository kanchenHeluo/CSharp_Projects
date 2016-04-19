using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallBackTool.Converter.Models
{
    public class AccountDBModel : IDBModel
    {
        #region Properties
        public string ActionCode { get; set; }
        public long AccountId { get; set; }
        public Guid AccountGuid { get; set; }
        [Required]
        public string AccountCode { get; set; }
        public string AccountShortDesc { get; set; }
        public string AccountLongDesc { get; set; }

        #endregion
    }
}
