using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SopraProject.Models.ObjectApi;

namespace SopraProject.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Returns the current authenticated user.
        /// </summary>
        protected User GetUser()
        {
            return (User)Session["User"];
        }
    }
}
