using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CRMRestService.Common
{
    public class CRMClient
    {
        #region properties
        private string crmResourceURL;
        private string crmServiceURL;
        private HttpClient httpClient;
        #endregion

        #region Constructuctors and DeConstructors
        public CRMClient(string authToken, string crmURL) 
        {
            crmServiceURL = Path.Combine(crmURL + "/", "XRMServices/2011/OrganizationData.svc/");

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        public CRMClient(HttpClientHandler handler, string crmURL)
        {
            crmServiceURL = Path.Combine(crmURL, "XRMServices/2011/OrganizationData.svc/");
            crmResourceURL = crmURL;

            httpClient = new HttpClient(handler);
        }

        ~CRMClient() { 
            httpClient.Dispose(); 
        }

        #endregion

        #region functions
        public async Task<string> CreateData(string jsonData, string entityName)
        {
            string url = crmServiceURL + entityName + "Set";
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Method = HttpMethod.Post;
            req.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            // Wait for the web service response.
            HttpResponseMessage response = await httpClient.SendAsync(req).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateData(string jsonData, string entityName, string guid)
        {
            string url = crmServiceURL + entityName + "Set";
            url += "(guid'" + guid + "')";
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Method = new HttpMethod("MERGE");
            req.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

            // Wait for the web service response.
            HttpResponseMessage response = await httpClient.SendAsync(req).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        // TODO: delete method

        public async Task<string> ReadData(string entityName, string filterKey, object filterValue, params string[] returnKeys)
        {
            string optionsStr = "?$select=" + ParseCollection(returnKeys);
            Guid id;
            if (filterValue.GetType().Name == "JValue" && (filterValue as JValue).Type.ToString().Equals("Integer"))
                optionsStr += "&$filter=" + filterKey + " eq " + filterValue;
            else if (Guid.TryParse(filterValue.ToString(), out id))
                optionsStr += "&$filter=" + filterKey + " eq guid%27" + filterValue + "%27";
            else
                optionsStr += "&$filter=" + filterKey + " eq %27" + filterValue + "%27";
            string url = crmServiceURL + entityName + "Set" + optionsStr;
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Method = HttpMethod.Get;

            HttpResponseMessage response = await httpClient.SendAsync(req).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMetaData()
        {
            try
            {
                string url = crmServiceURL + "$metadata";
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Method = HttpMethod.Get;

                // Wait for the web service response.
                HttpResponseMessage response = await httpClient.SendAsync(req).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return string.Empty;
            }
        }

        public async Task<string> SetState(string entityName, string stateCode, string statusCode, string guid)
        {
            try
            {
                string url = Path.Combine(crmResourceURL, "XRMServices/2011/Organization.svc/web");
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Method = HttpMethod.Post;

                StringBuilder sb = new StringBuilder();
                sb.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
                sb.Append("<s:Body>");
                sb.Append("<Execute xmlns=\"http://schemas.microsoft.com/xrm/2011/Contracts/Services\"");
                sb.Append(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">");
                sb.Append("<request i:type=\"b:SetStateRequest\"");
                sb.Append(" xmlns:a=\"http://schemas.microsoft.com/xrm/2011/Contracts\"");
                sb.Append(" xmlns:b=\"http://schemas.microsoft.com/crm/2011/Contracts\">");
                sb.Append("<a:Parameters xmlns:c=\"http://schemas.datacontract.org/2004/07/System.Collections.Generic\">");
                sb.Append("<a:KeyValuePairOfstringanyType>");
                sb.Append("<c:key>EntityMoniker</c:key>");
                sb.Append("<c:value i:type=\"a:EntityReference\">");
                sb.Append("<a:Id>" + guid + "</a:Id>");
                sb.Append("<a:LogicalName>" + entityName.ToLower() + "</a:LogicalName>");
                sb.Append("<a:Name i:nil=\"true\" />");
                sb.Append("</c:value>");
                sb.Append("</a:KeyValuePairOfstringanyType>");
                sb.Append("<a:KeyValuePairOfstringanyType>");
                sb.Append("<c:key>State</c:key>");
                sb.Append("<c:value i:type=\"a:OptionSetValue\">");
                sb.Append("<a:Value>" + stateCode + "</a:Value>");
                sb.Append("</c:value>");
                sb.Append("</a:KeyValuePairOfstringanyType>");
                sb.Append("<a:KeyValuePairOfstringanyType>");
                sb.Append("<c:key>Status</c:key>");
                sb.Append("<c:value i:type=\"a:OptionSetValue\">");
                sb.Append("<a:Value>" + statusCode + "</a:Value>");
                sb.Append("</c:value>");
                sb.Append("</a:KeyValuePairOfstringanyType>");
                sb.Append("</a:Parameters>");
                sb.Append("<a:RequestId i:nil=\"true\" />");
                sb.Append("<a:RequestName>SetState</a:RequestName>");
                sb.Append("</request>");
                sb.Append("</Execute>");
                sb.Append("</s:Body>");
                sb.Append("</s:Envelope>");

                req.Content = new StringContent(sb.ToString(), System.Text.Encoding.UTF8, "text/xml");
                req.Headers.Add("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/Execute");

                HttpResponseMessage response = await httpClient.SendAsync(req).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        private string ParseCollection(string[] returnKeys)
        {
            string str = string.Empty;
            foreach (string key in returnKeys)
            {
                if (!string.IsNullOrEmpty(str))
                    str += ",";
                str += key;
            }
            return str;
        }

    }
}