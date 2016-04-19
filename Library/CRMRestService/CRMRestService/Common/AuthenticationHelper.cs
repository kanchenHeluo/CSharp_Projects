using CRMRestService.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;

namespace CRMRestService.Common
{
    public static class AuthenticationHelper
    {
        public static string AuthToken()
        {
            ServiceSetting setting = new ServiceSetting();
            return AuthToken(setting);
        }
        public static string AuthToken(ServiceSetting setting)
        {
            string authority = String.Format(CultureInfo.InvariantCulture, setting.AadInstance, setting.Tenant);
            AuthenticationContext authContext = new AuthenticationContext(authority);
            AuthenticationResult authResult;

            // first, try to get a token silently
            try
            {
                authResult = authContext.AcquireTokenSilent(setting.CrmResourceURL, setting.ClientId);
            }
            catch (AdalException ex)
            {
                // There is no token in the cache; prompt the user to sign-in.
                if (ex.ErrorCode == "failed_to_acquire_token_silently")
                {
                    var ucredential = new UserCredential(setting.User, setting.Password);
                    authResult = authContext.AcquireToken(setting.CrmResourceURL, setting.ClientId, ucredential);
                }
                else throw;
            }

            return authResult.AccessToken;
        }

        public static HttpClientHandler GetHandler()
        {
            ServiceSetting setting = new ServiceSetting();
            return GetHandler(setting);
        }
        public static HttpClientHandler GetHandler(ServiceSetting setting)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential(setting.User, setting.Password);

            return handler;
        }

    }
}