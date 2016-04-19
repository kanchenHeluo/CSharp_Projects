using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class OpportunityItem
    {

        public List<OpportunityLineItem> SourceLineItems { get; set; }

        /// <summary>
        /// Collection of line items that are the children of the SourceLineItems collection.  A child can
        /// have multiple parents, which means that all of the line items in the SourceLineItems collection are considered the 
        /// parent for each line item in the TargetLineItems collection.  This collection can be empty.
        /// </summary>

        public List<OpportunityLineItem> TargetLineItems { get; set; }
    }
}
