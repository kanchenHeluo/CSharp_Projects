using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace MemoRestfulAPI.Models
{
    internal static class AadHelper
    {
        internal static void CheckClaims()
        {
            var scopeClaim = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/scope");

            if (scopeClaim != null && scopeClaim.Value != "user_impersonation")
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ReasonPhrase = "The Scope claim does not contain 'user_impersonation' or scope claim not found.",
                });
            }
        }
    }
}