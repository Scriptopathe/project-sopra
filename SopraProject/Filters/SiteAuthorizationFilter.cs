using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace SopraProject
{
    /// <summary>
    /// Authorization filter attribute.
    /// </summary>
    public class SiteAuthorizationFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Value indicating if authorization filtering is active.
        /// This value must be set to true in the production environnment.
        /// 
        /// Set it to false to be able to debug programs quickly.
        /// </summary>
        public const bool ENABLE_FILTERING = true;
        private bool _adminOnly;

        public SiteAuthorizationFilterAttribute(bool adminOnly = false)
        {
            _adminOnly = adminOnly;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (ENABLE_FILTERING)
            {
                string authTicket = (string)filterContext.HttpContext.Session["AuthTicket"];
                HttpCookie authCookie = filterContext.HttpContext.Request.Cookies["AuthTicket"];
                ObjectApi.User user = filterContext.HttpContext.Session["User"] as ObjectApi.User;
                if (authTicket == null || authCookie == null || authTicket != authCookie.Value || (_adminOnly && !user.IsAdmin))
                {
                    // filterContext.Result = new ContentResult() { Content = "Authentication failed" };
                    filterContext.Result = new RedirectResult("/site/signin?next=" + filterContext.HttpContext.Request.Url.AbsoluteUri);
                }
            }
            else
            {
                // Log in with a default user.
                if(filterContext.HttpContext.Session["User"] == null)
                    filterContext.HttpContext.Session["User"] = ObjectApi.User.Authenticate("User1", "User1pass");
            }
        }
    }
}

