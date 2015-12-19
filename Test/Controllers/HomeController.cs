using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
namespace Test.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var mvcName = typeof(Controller).Assembly.GetName();
			var isMono = Type.GetType("Mono.Runtime") != null;
			ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData["Runtime"] = isMono ? "Mono" : ".NET";
			Response.Write("Salut");
			return View("Empty");
		}

		// ~/home/eat
		public ActionResult Eat()
		{
			Response.Write("On peut imaginer que ça sera un fichier XML LOL.");

			return View("Empty");
		}

		public string[] Get()
		{
			return new string[]
			{
				"Hello",
				"World"
			};
		}
	}
}

