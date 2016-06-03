using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Efiling.Common;

namespace Efiling
{
    [DataContract(Namespace = "")]
    public partial class Efile
    {
        [NonSerialized, IgnoreDataMember]
        internal IDictionary<string, string> InternalFields;

        [DataMember(IsRequired = true)]
        public long Id { get; set; }

        [DataMember(IsRequired = true)]
        public USState State { get; set; }

        [DataMember(IsRequired = true)]
        public string Form { get; set; }

        [DataMember(Name="Name")]
        public string EfileName { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public Guid? TransmissionId { get; set; }
        public DateTime CreatedTime { get; set; }

        public bool IsEpay { get; set; }

        public string DataKey
        {
            get { return this.GetDataKey(); }
        }

    }
}
