using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using Web.Localization;
using Web.UI.Common.Exceptions;

namespace Web.UI.Common
{
    public class WebBaseController : Controller
    {
        /// <summary>
        /// Controller will by default return this value via Json method if recoverable exception has happed
        /// only apply to Json result actions and for none fatal erros
        /// </summary>
        protected object DefaultJsonReturn { get; set; }

        /// <summary>
        /// Message Field which will be returned to client by json result
        /// </summary>
        protected string Message { get; set; }

        /// <summary>
        /// Error Message Field which will be returned to client by json result
        /// </summary>
        protected string Error { get; set; }

        public WebBaseController()
        {
         
        }

        #region override methods

        /// <summary>
        /// 
        /// </summary>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Global exception handler, this handles recoverable exceptions for ajax request
        /// </summary>
        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception as OrderPortalException;
            var isAjax = filterContext.HttpContext.Request.IsAjaxRequest();

            if (!isAjax || ex == null || !ex.Recoverable)
            {
                base.OnException(filterContext);
                return;
            }

            Error = Error ?? ex.Message;
            filterContext.Result = Json(DefaultJsonReturn);
            filterContext.HttpContext.Server.ClearError();
            filterContext.ExceptionHandled = true;
        }

        #endregion

        /// <summary>
        /// Override the default json data to wrap some base information
        /// </summary>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            var ret = new JsonNetResult()
            {
                Data = new
                {
                    Message = Message.Localize(),                    
                    Error = Error.Localize(),
                    Data = data,
                    HasBase = true
                },
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue,
            };
            return ret;
        }

        /// <summary>
        /// For Json result to redirect. since the action redirect will render the target html to the respons, return an url to the client instead. client side Js script will handle the actual redirect
        /// </summary>
        protected JsonResult RedirectJson(string url)
        {
            var ret = new JsonNetResult()
            {
                Data = new { IsRedirect = true, RedirectLocation = url },
                MaxJsonLength = Int32.MaxValue,
            };
            return ret;
        }

        protected JsonResult InvalidRequest(string errorMessage = null)
        {
            Error = errorMessage ?? "Invalid request";
            return Json(DefaultJsonReturn);
        }
    }
}