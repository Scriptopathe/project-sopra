using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace SopraProject
{
    /// <summary>
    /// Authorization filter attribute.
    /// </summary>
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public AuthorizationFilterAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string authTicket = (string)filterContext.HttpContext.Session["AuthTicket"];
            HttpCookie authCookie = filterContext.HttpContext.Request.Cookies["AuthTicket"];
            if (authTicket == null || authCookie == null || authTicket != authCookie.Value)
            {
                // filterContext.Result = new ContentResult() { Content = "Authentication failed" };
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}

