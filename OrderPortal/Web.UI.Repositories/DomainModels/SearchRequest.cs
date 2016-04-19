using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class SearchRequest
    {
        public int PageNumber { get; set; }
        public int NoOfRecords { get; set; }

        /// </summary>
        public string SearchText { get; set; }

        public SearchPattern SearchPattern { get; set; }

    }

    public enum SearchPattern
    {
        Like,
        Exact
    }
}
