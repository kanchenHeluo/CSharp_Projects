
using Demo.RestAPI.Common;

namespace Demo.RestAPI.Attributes
{
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class RequireAuthorization : AuthorizationFilterAttribute
    {
        private readonly bool certificateRequired = true;

        #region Public Methods and Operators

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // Determines wether to enable client certificate check.
            if (certificateRequired)
            {
                X509Certificate2 cert = actionContext.Request.GetClientCertificate();
                if (cert == null || !cert.Verify())
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        ReasonPhrase =
                            "Valid client certificate required"
                    };
                    return;
                }

                string authorizedCertificate = ConfigurationManager.AppSettings[Constants.ClientCertificateThumbprint].Trim().ToLower();
                

                if (string.IsNullOrWhiteSpace(authorizedCertificate) || string.IsNullOrWhiteSpace(cert.Thumbprint)
                    || !authorizedCertificate.Equals(cert.Thumbprint.ToLower()))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        ReasonPhrase =
                            "Not authorized certificate"
                    };
                    return;
                }
            }

            base.OnAuthorization(actionContext);
        }

        #endregion
    }
}