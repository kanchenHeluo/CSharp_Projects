using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Web.UI.Repositories.DomainModels
{
    #region Enum
    /// <summary>
    /// Identifies the Requested PriceType at the HeaderLevel
    /// </summary>
   public enum HeaderType
    {
        /// <summary>
        /// Represents PricePoint estimate request
        /// </summary>
        OrderEstimate = 0,
        Order=1
    }
    #endregion
}
