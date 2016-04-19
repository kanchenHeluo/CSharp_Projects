using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Web.UI.Repositories.DomainModels
{
    #region Class
    /// <summary>
    /// Represents the details of any dynamic or configurable attributes
    /// </summary>
   
    public class ExtendedProperty 
    {
        #region Private Declaration
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or Sets the name of attribute that is added in extended collection
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets the value of attribute that is added in extended collection
        /// </summary>
        public object Value { get; set; }
        #endregion
    }
    #endregion
}
