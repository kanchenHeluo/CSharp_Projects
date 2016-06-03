using System;
using Efiling.Common;

namespace Efiling
{
    public class EfileSummary
    {
        public long? Id { get; set; }
        public Guid? TransmissionId { get; set; }
        public EfileStatus Status { get; set; }

        public string State { get; set; }

        public string EfileType { get; set; }

        public EfileSummary(Efile setting)
        {
            State = setting.State.ToString();
            EfileType = setting.Form;
            Status = EfileStatus.ValidationFailed;
        }

        public EfileSummary()
        {
            Status = EfileStatus.ValidationFailed;
        }
    }
}
