using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Web.Security;
namespace SopraProject.Controllers
{
    /// <summary>
    /// API controler.
    /// </summary>
    public class ApiController : BaseController
    {
        public class Test
        {
            public string Str { get; set; }
            public string Truc { get; set; }
            public Test() { }
        }

        /// <summary>
        /// Authenticates an user.
        /// </summary>
        public ActionResult Authenticate()
        {
            SopraProject.ObjectApi.User usr = SopraProject.ObjectApi.User.Authenticate(
                new SopraProject.UserIdentifier(Request.QueryString["username"]), 
                Request.QueryString["password"]);

            /*if (usr == null)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                                      // ticket version
                    Request.QueryString["username"],        // authenticated username
                    DateTime.Now,                           // issueDate
                    DateTime.Now.AddDays(30),               // expiryDate
                    true,                                   // true to persist across browser sessions
                    null,                                   // can be used to store additional user data
                    FormsAuthentication.FormsCookiePath);   // the path for the cookie

                // Encrypt the ticket using the machine key
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // Add the cookie to the request to save it
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                cookie.HttpOnly = true; 
                Response.Cookies.Add(cookie);

                // Your redirect logic
                // Response.Redirect(FormsAuthentication.GetRedirectUrl(username, isPersistent));
            }*/

            return Content(usr == null ? "Wrong Username / Password" : "Authenticated");
        }

        /// <summary>
        /// Creates the db.
        /// </summary>
        /// <returns>The db.</returns>
        public ActionResult CreateDb()
        {
            Database.DatabaseWorker.CreateDatabase();
            return Content("Database successfully created.");
        }

        public ActionResult Rooms()
        {
            //var rooms = SopraProject.ObjectApi.Room.GetAllRooms();
            // Response.Write(Serializer.Serialize<List<ObjectApi.Room>>(rooms));

            return Content(Tools.Serializer.Serialize<Test>(new Test() { Str = "hahaha", Truc = "kokoko" }));
        }
    }
}

