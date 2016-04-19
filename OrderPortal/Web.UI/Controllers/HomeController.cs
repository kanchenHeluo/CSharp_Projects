using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Web.UI.Common;
using System.Web.Security;
using System.IdentityModel.Services;
using System.Security.Authentication;
using System.Security.AccessControl;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using MS.IT.Ops.Shared.Security.SecureWebClient;
using System.Configuration;
using System.IdentityModel.Services.Configuration;
using Web.UI.UnitOfWork;
using Microsoft.Practices.Unity;

namespace Web.UI.Controllers
{
    public class HomeController : WebBaseController
    {

  

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LoginPartner()
        {
            string uniqueId = Guid.NewGuid().ToString();
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            fam.HomeRealm = GetRealmValue()[1];                
            fam.Issuer = FederatedAuthentication.WSFederationAuthenticationModule.Issuer;
            fam.Realm = FederatedAuthentication.WSFederationAuthenticationModule.Realm;
            fam.Reply = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.Reply;
            var message = fam.CreateSignInRequest(uniqueId, fam.Reply, false);
            Response.Redirect(message.WriteQueryString());
            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult LoginWindowsLive()
        {
            string uniqueId = Guid.NewGuid().ToString();
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            fam.HomeRealm = GetRealmValue()[0];
            fam.Issuer = FederatedAuthentication.WSFederationAuthenticationModule.Issuer;
            fam.Realm = FederatedAuthentication.WSFederationAuthenticationModule.Realm;
            fam.Reply = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.Reply;
            fam.AuthenticationType = fam.HomeRealm;
            var message = fam.CreateSignInRequest(uniqueId, fam.Reply, false);            
            Response.Redirect(message.WriteQueryString());
            return new EmptyResult();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult SignOut()
        {
            Session.ClearSession();
            FederatedAuthentication.SessionAuthenticationModule.SignOut();              
            FormsAuthentication.SignOut();
            IdentityManager identMgr = IdentityManager.GetInstance();
            identMgr.Logout();         
            HandleExternalSignout();
            return View("Login");
        }

        /// <summary>
        /// Method for UnAuthorized user.
        /// </summary>
        /// <returns></returns>
        public ActionResult UnAuthorized()
        {
            return View("UnAuthorized");
        }
        private void HandleExternalSignout()
        {
            try
            {                
                if (Thread.CurrentPrincipal.Identity.AuthenticationType == "Federation")
                {
                        SignOutCorpSTS();
                }
            }
            catch (ThreadAbortException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets HomeRealm values from configuration file.
        /// </summary>
        /// <returns>
        /// Returns string array by splitting the value based on ','
        /// </returns>
        private string[] GetRealmValue()
        {
            return FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.HomeRealm.Split(new char[] { ',' });
        }

        /// <summary>
        /// Signs the out CorpSTS.
        /// </summary>
        private void SignOutCorpSTS()
        {
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            WSFederationAuthenticationModule.FederatedSignOut(new System.Uri(fam.Issuer), new System.Uri(FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.SignOutReply));
        }       

    }
}