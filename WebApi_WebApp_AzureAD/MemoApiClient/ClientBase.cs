using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MemoApiClient
{
    public class ClientBase
    {
        static ClientBase()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    return true;
                };
        }
        protected ClientBase() { }

        private async Task<HttpClient> CreateHttpClientAsync()
        {
            var httpClient = new HttpClient();
            var authContext = new AuthenticationContext(Config.AADInstance);
            var clientCredential = new ClientCredential(Config.ClientId, Config.AppKey);
            var result = await authContext.AcquireTokenAsync(Config.ServiceResourceId, clientCredential);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            return httpClient;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            var authContext = new AuthenticationContext(Config.AADInstance);
            var clientCredential = new ClientCredential(Config.ClientId, Config.AppKey);
            var result = authContext.AcquireToken(Config.ServiceResourceId, clientCredential);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            return httpClient;
        }

        protected async Task<T> GetJsonAsync<T>(Uri uri)
        {
            var client = await CreateHttpClientAsync();
            
            var getAsyncTask = await client.GetAsync(uri);
            var response = getAsyncTask.EnsureSuccessStatusCode();

            var readAsStringAsyncTask = await response.Content.ReadAsStringAsync();

            var serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<T>(readAsStringAsyncTask);

            return obj;
        }

        protected T GetJson<T>(Uri uri)
        {
            var client = CreateHttpClient();

            var getAsyncTask = client.GetAsync(uri);
            getAsyncTask.Wait();

            var response = getAsyncTask.Result.EnsureSuccessStatusCode();

            var readAsStringAsyncTask = response.Content.ReadAsStringAsync();
            readAsStringAsyncTask.Wait();

            var s = readAsStringAsyncTask.Result;

            var serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<T>(s);

            return obj;
        }

    }
}
