

using System;
using System.Collections.Generic;
using Efiling.Common;

namespace Efiling
{
    public partial class EfileUtil
    {
        public static EfileSummary TransmitEfile(this Efile setting, IDictionary<string, string> input)
        {

            EfileSummary summary = new EfileSummary(setting);
            //validate
            IList<string> logs = new List<string>();
            if (!setting.InternalFields.ValidateInput(input, logs))
            {
                return null;
            }
            //call service to generate
            Efile efile = _svc.GenerateEfile(input);
            summary.TransmissionId = efile.TransmissionId;
            if (Guid.Empty == efile.TransmissionId)
            {
                return null;
            }
            //save to db
            summary.Id = EfileHistory.SaveEfileHistory(efile);

            return summary;
        }
    }
}
