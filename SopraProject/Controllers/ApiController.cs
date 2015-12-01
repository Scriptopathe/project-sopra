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
        /// The request must have a username and password in the GET parameters.
        /// </summary>
        public ActionResult Login()
        {
            string username = Request["username"];
            string password = Request["password"];
            if (username == null || password == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
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
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
        }
        /// <summary>
        /// Logs out an user.
        /// </summary>
        [AuthorizationFilter]
        public ActionResult Logout()
        {
            Session["AuthTicket"] = null;
            Session["Username"] = null;
            Response.Cookies.Remove("AuthTicket");
            return new HttpStatusCodeResult(200);
        }

        /// <summary>
        /// Creates the db.
        /// </summary>
        /// <returns>The db.</returns>
        [AuthorizationFilter]
        public ActionResult CreateDb()
        {
            Database.DatabaseWorker.CreateDatabase();
            return Content("Database successfully created.");
        }

        /// <summary>
        /// TEST
        /// </summary>
        [AuthorizationFilter]
        public ActionResult PrintUser()
        {
            var user = GetUser();
            return Content("Username is " + user.Username);
        }

        [AuthorizationFilter]
        public ActionResult Rooms()
        {
            //var rooms = SopraProject.ObjectApi.Room.GetAllRooms();
            // Response.Write(Serializer.Serialize<List<ObjectApi.Room>>(rooms));

            return Content(Tools.Serializer.Serialize<Test>(new Test() { Str = "hahaha", Truc = "kokoko" }));
        }
    }
}

