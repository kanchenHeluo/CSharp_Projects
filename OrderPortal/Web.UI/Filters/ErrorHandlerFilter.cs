using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.UI.Common;
namespace Web.UI.Filters
{
    public class ErrorHandlerFilter :HandleErrorAttribute
    {
        #region Private Methods

        /// <summary>
        /// Logs Exception using ECIT Logging context
        /// </summary>
        /// <param name="exception">current exception which caused the issue</param>
        private void LogException(Exception exception)
        {
            // TODO: <Insert justification for suppressing CS0618>
            //Microsoft.IT.Licensing.Diagnostics.Common.ApplicationDiagnostics.WriteException(exception, TraceEventType.Error, 26002);
            // TODO: <Insert justification for suppressing CS0618>
            //Microsoft.IT.Licensing.Diagnostics.Common.ApplicationDiagnostics.WriteException(exception, TraceEventType.Information, 26003);

        }     

        #endregion

        #region Public Methods
        /// <summary>
        /// Handles AJAX Exception
        /// </summary>
        /// <param name="filterContext"><see cref="ExceptionContext"/> exception context</param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;
            // Should consider ajax only request in release mode?
            if (filterContext.Exception != null)
            {
                LogException(filterContext.Exception);

                var statusCode = (int)HttpStatusCode.InternalServerError;
                if (filterContext.Exception is HttpException)
                {
                    statusCode = ((HttpException)filterContext.Exception).GetHttpCode();
                }
                else if (filterContext.Exception is UnauthorizedAccessException)
                {
                    //to prevent login prompt in IIS
                    // which will appear when returning 401.
                    statusCode = (int)HttpStatusCode.Forbidden;
                }

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = statusCode;
                    // Ajax Request return ajax
                    filterContext.Result = new JsonResult()
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                       
                    };
                    filterContext.ExceptionHandled = true;
                }
                else
                {
                    var result = CreateActionResult(filterContext, statusCode);
                    if (ConfigurationManager.AppSettings["ShowExceptionDetailToConsole"] == "true")
                    {
                        var ex = filterContext.Exception.InnerException ?? filterContext.Exception;
                        var message = String.Format("Error: {0} \r\n, Stack Trace: {1}", ex.Message, ex.StackTrace);
                        filterContext.Result = new JsonResult(){Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
                    }
                    else
                    {
                        filterContext.Result = result;
                    }

                    // Prepare the response code.
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = statusCode;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

                }
            }
            else
            {
                base.OnException(filterContext);
            }
        }

        protected virtual ActionResult CreateActionResult(ExceptionContext filterContext, int statusCode)
        {
            var ctx = new ControllerContext(filterContext.RequestContext, filterContext.Controller);
            var statusCodeName = ((HttpStatusCode)statusCode).ToString();

            var viewName = SelectFirstView(ctx,
                                           string.Format("~/Views/Error/{0}.cshtml", statusCodeName),
                                           "~/Views/Error/Error.cshtml",
                                           statusCodeName,
                                           "Error");

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var result = new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
            };
            result.ViewBag.StatusCode = statusCode;
            if (statusCode != (int)HttpStatusCode.Forbidden)
            {
                result.ViewBag.ErrorHeading = StringHelper.GetString("ServiceErrorHeading");
                result.ViewBag.ErrorSubHeading = StringHelper.GetString("ServiceErrorSubHeading") + "" + StringHelper.GetString("ErrorSupportMsg");
            }
            else
            {
                result.ViewBag.ErrorHeading = StringHelper.GetString("NotAuthErrorMessage");
                result.ViewBag.ErrorSubHeading = StringHelper.GetString("ErrorSupportMsg");
            }
            return result;
        }

        protected string SelectFirstView(ControllerContext ctx, params string[] viewNames)
        {
            return viewNames.First(view => ViewExists(ctx, view));
        }

        protected bool ViewExists(ControllerContext ctx, string name)
        {
            var result = ViewEngines.Engines.FindView(ctx, name, null);
            return result.View != null;
        }
        #endregion
    }
}