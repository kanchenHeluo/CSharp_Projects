using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Security
{
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class ContactClaim
        {

            private ContactClaimLocalContactClaim localContactClaimField;

            private string contactGuidField;

            private string publicNumberField;

            /// <remarks/>
            public ContactClaimLocalContactClaim LocalContactClaim
            {
                get
                {
                    return this.localContactClaimField;
                }
                set
                {
                    this.localContactClaimField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactGuid
            {
                get
                {
                    return this.contactGuidField;
                }
                set
                {
                    this.contactGuidField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PublicNumber
            {
                get
                {
                    return this.publicNumberField;
                }
                set
                {
                    this.publicNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ContactClaimLocalContactClaim
        {

            private ContactClaimLocalContactClaimOrganizationClaims[] organizationClaimsField;

            private string givenNameField;

            private string familyNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("OrganizationClaims")]
            public ContactClaimLocalContactClaimOrganizationClaims[] OrganizationClaims
            {
                get
                {
                    return this.organizationClaimsField;
                }
                set
                {
                    this.organizationClaimsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string GivenName
            {
                get
                {
                    return this.givenNameField;
                }
                set
                {
                    this.givenNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FamilyName
            {
                get
                {
                    return this.familyNameField;
                }
                set
                {
                    this.familyNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ContactClaimLocalContactClaimOrganizationClaims
        {

            private ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaim localOrganizationClaimField;

            private string organizationGuidField;

            private string publicNumberField;

            /// <remarks/>
            public ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaim LocalOrganizationClaim
            {
                get
                {
                    return this.localOrganizationClaimField;
                }
                set
                {
                    this.localOrganizationClaimField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string OrganizationGuid
            {
                get
                {
                    return this.organizationGuidField;
                }
                set
                {
                    this.organizationGuidField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string PublicNumber
            {
                get
                {
                    return this.publicNumberField;
                }
                set
                {
                    this.publicNumberField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaim
        {

            private ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaimContactRoleClaims contactRoleClaimsField;

            private string externalNameField;

            private string internalNameField;

            /// <remarks/>
            public ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaimContactRoleClaims ContactRoleClaims
            {
                get
                {
                    return this.contactRoleClaimsField;
                }
                set
                {
                    this.contactRoleClaimsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ExternalName
            {
                get
                {
                    return this.externalNameField;
                }
                set
                {
                    this.externalNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string InternalName
            {
                get
                {
                    return this.internalNameField;
                }
                set
                {
                    this.internalNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaimContactRoleClaims
        {

            private ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaimContactRoleClaimsContactRoleTypeClaim contactRoleTypeClaimField;

            private uint contactRoleIdField;

            /// <remarks/>
            public ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaimContactRoleClaimsContactRoleTypeClaim ContactRoleTypeClaim
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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint ContactRoleId
            {
                get
                {
                    return this.contactRoleIdField;
                }
                set
                {
                    this.contactRoleIdField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ContactClaimLocalContactClaimOrganizationClaimsLocalOrganizationClaimContactRoleClaimsContactRoleTypeClaim
        {

            private byte contactRoleTypeIdField;

            private string contactRoleTypeNameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte ContactRoleTypeId
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

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ContactRoleTypeName
            {
                get
                {
                    return this.contactRoleTypeNameField;
                }
                set
                {
                    this.contactRoleTypeNameField = value;
                }
            }
        }


   
}