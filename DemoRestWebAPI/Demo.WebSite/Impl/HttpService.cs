
namespace Demo.WebSite.Impl
{
    using Interface;
    using Common;

    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpService : IHttpService
    {
        public async Task<string> HttpsGetAsync(Uri requestUri)
        {
            return await this.HttpsGetAsync(requestUri, CancellationToken.None);
        }

        public async Task<string> HttpsGetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            using (HttpClient httpClient = GetHttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);

                var content = await response.Content.ReadAsStringAsync();
                CheckHttpResponse(response, content);
                return content;
            }
        }

        #region Methods

        private static void CheckHttpResponse(HttpResponseMessage response, string content)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private static HttpClient GetHttpClient()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            var clientCertificateThumbprint = ConfigurationManager.AppSettings[Constants.ClientCertificateThumbprint];

            var cert = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(
                        certificate =>
                        certificate.Thumbprint != null
                        && certificate.Thumbprint.Equals(clientCertificateThumbprint, StringComparison.CurrentCultureIgnoreCase)
                        && certificate.Verify());//&& certificate.Verify()

            if (cert == null)
            {
                throw new Exception("No valid certificate installed.");
            }

            var webRequestHandler = new WebRequestHandler { UseDefaultCredentials = true, AllowPipelining = true };
            webRequestHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            webRequestHandler.ClientCertificates.Add(cert);

            var httpClient = new HttpClient(webRequestHandler);            

            return httpClient;
        }

        #endregion
    }
}