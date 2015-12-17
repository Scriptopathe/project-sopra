using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SopraProject.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Returns the current authenticated user.
        /// </summary>
        protected ObjectApi.User GetUser()
        {
            return (ObjectApi.User)Session["User"];
        }
    }
}
