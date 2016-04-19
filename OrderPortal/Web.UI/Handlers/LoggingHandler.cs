using Microsoft.IT.Licensing.Diagnostics.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Web.UI.Common;

namespace Web.UI.Handlers
{

    public class LoggingHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                try
                {
                    var requestContent = await request.Content.ReadAsStringAsync();
                    ApplicationDiagnostics.WriteMessage(string.Format("Request: {0}, Content: {1}",
                        request, requestContent), TraceEventType.Verbose, Constants.RequestLogEventId, "LoggingHandler");
                }
                catch
                {
                    //do nothing
                }
            }
            var response = await base.SendAsync(request, cancellationToken);


            if (response.Content != null)
            {
                try
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    ApplicationDiagnostics.WriteMessage(string.Format("Response: {0}, Content: {1}",
                        response, responseContent), TraceEventType.Verbose, Constants.ResponseLogEventId,
                        "LoggingHandler");
                }
                catch 
                {
                    //do nothing
                }
            }

            return response;
        }
    }
}