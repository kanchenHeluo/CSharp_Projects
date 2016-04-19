using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Web.UI.Repositories.DomainModels
{
    #region Class
    /// <summary>
    /// Represents the details of any Issue/error occurred while pricing the Quote item(s)
    /// </summary>
   
    public class IssueInfo 
    {
        #region Private Declaration
        private Guid? lineItemGuid;
        #endregion

        #region Public Properties
        /// <summary>
        /// Unique Identifier of the LineItem
        /// </summary>
        public Guid? LineItemGuid
        {
            get
            {
                return lineItemGuid;
            }
            set
            {
                lineItemGuid = value;
            }
        }

        /// <summary>
        /// Name of the Object/Process that triggered the issue/error
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Unique Number of the Issue
        /// </summary>
        public int? IssueNumber { get; set; }

        /// <summary>
        /// Detailed comment about the issue/error
        /// </summary>
        public string IssueComment { get; set; }
        #endregion
    }
    #endregion
}
