using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Security
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/", IsNullable = false)]
    public partial class AccessorResponse
    {

        private AccessorResponseAccessor accessorField;

        /// <remarks/>
        public AccessorResponseAccessor Accessor
        {
            get
            {
                return this.accessorField;
            }
            set
            {
                this.accessorField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessor
    {

        private string accessorGuIDField;

        private uint changeControlField;

        private string applicationGuIDField;

        private string credentialTypeField;

        private string credentialValueField;

        private AccessorResponseAccessorAccessorStatus accessorStatusField;

        private object localeCodeField;

        private AccessorResponseAccessorAccessorCredentials accessorCredentialsField;

        private AccessorResponseAccessorAccessorClaim[] accessorClaimsField;

        private AccessorResponseAccessorAccessorRoles accessorRolesField;

        /// <remarks/>
        public string AccessorGuID
        {
            get
            {
                return this.accessorGuIDField;
            }
            set
            {
                this.accessorGuIDField = value;
            }
        }

        /// <remarks/>
        public uint ChangeControl
        {
            get
            {
                return this.changeControlField;
            }
            set
            {
                this.changeControlField = value;
            }
        }

        /// <remarks/>
        public string ApplicationGuID
        {
            get
            {
                return this.applicationGuIDField;
            }
            set
            {
                this.applicationGuIDField = value;
            }
        }

        /// <remarks/>
        public string CredentialType
        {
            get
            {
                return this.credentialTypeField;
            }
            set
            {
                this.credentialTypeField = value;
            }
        }

        /// <remarks/>
        public string CredentialValue
        {
            get
            {
                return this.credentialValueField;
            }
            set
            {
                this.credentialValueField = value;
            }
        }

        /// <remarks/>
        public AccessorResponseAccessorAccessorStatus AccessorStatus
        {
            get
            {
                return this.accessorStatusField;
            }
            set
            {
                this.accessorStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object LocaleCode
        {
            get
            {
                return this.localeCodeField;
            }
            set
            {
                this.localeCodeField = value;
            }
        }

        /// <remarks/>
        public AccessorResponseAccessorAccessorCredentials AccessorCredentials
        {
            get
            {
                return this.accessorCredentialsField;
            }
            set
            {
                this.accessorCredentialsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AccessorClaim", IsNullable = false)]
        public AccessorResponseAccessorAccessorClaim[] AccessorClaims
        {
            get
            {
                return this.accessorClaimsField;
            }
            set
            {
                this.accessorClaimsField = value;
            }
        }

        /// <remarks/>
        public AccessorResponseAccessorAccessorRoles AccessorRoles
        {
            get
            {
                return this.accessorRolesField;
            }
            set
            {
                this.accessorRolesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorStatus
    {

        private byte statusIDField;

        private string statusNameField;

        /// <remarks/>
        public byte StatusID
        {
            get
            {
                return this.statusIDField;
            }
            set
            {
                this.statusIDField = value;
            }
        }

        /// <remarks/>
        public string StatusName
        {
            get
            {
                return this.statusNameField;
            }
            set
            {
                this.statusNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorCredentials
    {

        private AccessorResponseAccessorAccessorCredentialsAccessorCredential accessorCredentialField;

        /// <remarks/>
        public AccessorResponseAccessorAccessorCredentialsAccessorCredential AccessorCredential
        {
            get
            {
                return this.accessorCredentialField;
            }
            set
            {
                this.accessorCredentialField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorCredentialsAccessorCredential
    {

        private string credentialTypeField;

        private string credentialValueField;

        /// <remarks/>
        public string CredentialType
        {
            get
            {
                return this.credentialTypeField;
            }
            set
            {
                this.credentialTypeField = value;
            }
        }

        /// <remarks/>
        public string CredentialValue
        {
            get
            {
                return this.credentialValueField;
            }
            set
            {
                this.credentialValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorClaim
    {

        private uint accessorExternalObjectIDField;

        private string typeField;

        private string valueField;

        /// <remarks/>
        public uint AccessorExternalObjectID
        {
            get
            {
                return this.accessorExternalObjectIDField;
            }
            set
            {
                this.accessorExternalObjectIDField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorRoles
    {

        private AccessorResponseAccessorAccessorRolesAccessorRole accessorRoleField;

        /// <remarks/>
        public AccessorResponseAccessorAccessorRolesAccessorRole AccessorRole
        {
            get
            {
                return this.accessorRoleField;
            }
            set
            {
                this.accessorRoleField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorRolesAccessorRole
    {

        private string typeField;

        private string valueField;

        private AccessorResponseAccessorAccessorRolesAccessorRoleConstraints constraintsField;

        private AccessorResponseAccessorAccessorRolesAccessorRoleApplicationRole[] applicationRolesField;

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public AccessorResponseAccessorAccessorRolesAccessorRoleConstraints Constraints
        {
            get
            {
                return this.constraintsField;
            }
            set
            {
                this.constraintsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ApplicationRole", IsNullable = false)]
        public AccessorResponseAccessorAccessorRolesAccessorRoleApplicationRole[] ApplicationRoles
        {
            get
            {
                return this.applicationRolesField;
            }
            set
            {
                this.applicationRolesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorRolesAccessorRoleConstraints
    {

        private AccessorResponseAccessorAccessorRolesAccessorRoleConstraintsConstraint constraintField;

        /// <remarks/>
        public AccessorResponseAccessorAccessorRolesAccessorRoleConstraintsConstraint Constraint
        {
            get
            {
                return this.constraintField;
            }
            set
            {
                this.constraintField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorRolesAccessorRoleConstraintsConstraint
    {

        private string typeField;

        private uint valueField;

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public uint Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorRolesAccessorRoleApplicationRole
    {

        private string typeField;

        private string valueField;

        private AccessorResponseAccessorAccessorRolesAccessorRoleApplicationRoleApplicationFunction[] applicationFunctionsField;

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ApplicationFunction", IsNullable = false)]
        public AccessorResponseAccessorAccessorRolesAccessorRoleApplicationRoleApplicationFunction[] ApplicationFunctions
        {
            get
            {
                return this.applicationFunctionsField;
            }
            set
            {
                this.applicationFunctionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorResponseAccessorAccessorRolesAccessorRoleApplicationRoleApplicationFunction
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }




   
}