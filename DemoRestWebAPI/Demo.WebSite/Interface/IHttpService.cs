
namespace Demo.WebSite.Interface
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    interface IHttpService
    {
        Task<string> HttpsGetAsync(Uri requestUri);

        Task<string> HttpsGetAsync(Uri requestUri, CancellationToken cancellationToken);

    }
}
