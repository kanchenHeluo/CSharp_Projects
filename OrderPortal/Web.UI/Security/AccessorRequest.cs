using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Security
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/", IsNullable = false)]
    public partial class AccessorRequest
    {

        private AccessorRequestAccessor accessorField;

        /// <remarks/>
        public AccessorRequestAccessor Accessor
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
    public partial class AccessorRequestAccessor
    {

        private string applicationGuIDField;

        private string credentialTypeField;

        private string credentialValueField;

        private AccessorRequestAccessorAccessorCredentials accessorCredentialsField;

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
        public AccessorRequestAccessorAccessorCredentials AccessorCredentials
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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://MS.IT.Ops.MSLicense.TransactionService.DataContracts/")]
    public partial class AccessorRequestAccessorAccessorCredentials
    {

        private AccessorRequestAccessorAccessorCredentialsAccessorCredential accessorCredentialField;

        /// <remarks/>
        public AccessorRequestAccessorAccessorCredentialsAccessorCredential AccessorCredential
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
    public partial class AccessorRequestAccessorAccessorCredentialsAccessorCredential
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



}