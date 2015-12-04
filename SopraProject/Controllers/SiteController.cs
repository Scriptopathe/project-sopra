using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SopraProject.Controllers
{
    public class SiteController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
