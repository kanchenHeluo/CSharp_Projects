using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Security
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ContactRoleClaim
    {

        private ContactRoleClaimContactRoleTypeClaim contactRoleTypeClaimField;

        /// <remarks/>
        public ContactRoleClaimContactRoleTypeClaim ContactRoleTypeClaim
        {
            get
            {
                return this.contactRoleTypeClaimField;
            }
            set
            {
                this.contactRoleTypeClaimField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ContactRoleClaimContactRoleTypeClaim
    {

        private string contactRoleTypeIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ContactRoleTypeId
        {
            get
            {
                return this.contactRoleTypeIdField;
            }
            set
            {
                this.contactRoleTypeIdField = value;
            }
        }
    }


}