using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SopraProject.Controllers
{
    public class SiteController : Controller
    {
        /// <summary>
        /// Views the index page.
        /// </summary>
        [SiteAuthorizationFilter]
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Views the sign in page.
        /// </summary>
        /// <returns>The in.</returns>
        [HttpGet]
        public ActionResult SignIn(string next="", string status="")
        {
            return View("SignIn");
        }

        /// <summary>
        /// Authenticates an user.
        /// The request must have a username and password in the POST parameters.
        /// </summary>
        public ActionResult Login(string next="")
        {
            string username = Request["username"];
            string password = Request["password"];
            if (username == null || password == null)
            {
                return RedirectToAction("SignIn", new { next = next, status = "Error : Wrong username or password."});
            }

            SopraProject.ObjectApi.User usr = SopraProject.ObjectApi.User.Authenticate(
                new SopraProject.UserIdentifier(username), 
                password);

            // Creates a session ticket for the user.
            if (usr != null)
            {
                string authId = Guid.NewGuid().ToString();  
                HttpCookie cookie = new HttpCookie("AuthTicket", authId) { HttpOnly = true };
                Response.Cookies.Add(cookie);
                Session["AuthTicket"] = authId;
                Session["Username"] = username;
                Session["User"] = usr;

                if (next != "")
                    return Redirect(next);
                else
                    return RedirectToAction("SignIn", new { next = next, status = "Successfully logged in."});
            }

            return RedirectToAction("SignIn", new { next = next, status = "Error : Wrong username or password."});
        }
    }
}
