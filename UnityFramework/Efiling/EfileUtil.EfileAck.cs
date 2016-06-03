namespace Efiling
{
    public partial class EfileUtil
    {
        public static EfileSummary UpdateAck(this Efile setting, long id, string content)
        {
            /*
            var h = EfileHistory.GetStateEfileHistory(id);
            var summary = new EfileSummary(setting);
            if (h != null)
            {
                summary.EfileId = h.Id;
                summary.Status = h.Status;
                try
                {
                    if (setting.Format == TextTemplateFormat.Xml && !setting.SkipAckParsing)
                    {
                        var response = XDocument.Parse(content);
                        var ret = setting.ParseSoapAckResponse(response, summary, h.TransmissionId);
                        if (ret.HasValue)
                        {
                            summary.Status = ret.Value ? EfileStatus.Accepted : EfileStatus.Rejected;
                            summary.Logs.AddString("Update successful");
                        }
                    }
                    else
                    {
                        if (!h.Save(EfileContentType.Ack, content))
                        {
                            summary.Logs.AddString("Unexpected Error");
                        }
                    }
                }
                catch (Exception e)
                {
                    summary.Logs.AddString(e.Message);
                    summary.Logs.AddString("Error parsing xml document, please make sure the file content is complete");
                }
            }
            return summary;*/
            return new EfileSummary();
        }
    }
}
