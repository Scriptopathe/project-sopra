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
                Session["User"] = usr;
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
            Session["User"] = null;
            Response.Cookies.Remove("AuthTicket");
            return new HttpStatusCodeResult(200);
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

        /// <summary>
        /// TEST
        /// </summary>
        [AuthorizationFilter]
        public ActionResult PrintUser()
        {
            var user = GetUser();
            return Content("Username is " + user.Username + " Location : " + user.Location.Name  + "@" + user.Location.Address);
        }

        /// <summary>
        /// Gets the current logged user name.
        /// </summary>
        /// <returns>The user.</returns>
        [AuthorizationFilter]
        public new ActionResult User()
        {
            var user = GetUser();
            return Content(user.Username);
        }

        [HttpPost]
        [AuthorizationFilter]
        public ActionResult Location(string siteId)
        {
            var user = GetUser();
            user.Location = new ObjectApi.Site(new SiteIdentifier(siteId.ToString()));
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Location()
        {
            var user = GetUser();
            return Content(user.Location.Identifier.Value);
        }

        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Rooms()
        {
            var rooms = SopraProject.ObjectApi.Room.GetAllRooms();
            return Content(Tools.Serializer.Serialize(rooms));
        }

        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Rooms(int roomId)
        {
            var room = new ObjectApi.Room(new RoomIdentifier(roomId.ToString()));
            return Content(Tools.Serializer.Serialize(room));
        }

        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Rooms(int siteId=-1, int personCount=-1, List<ParticularityIdentifier> particularities=null)
        {
            var rooms = SopraProject.ObjectApi.Room.GetAllRooms();
            if (siteId != -1)
            {
                //List<SopraProject.ObjectApi.Site> sites = null;
                for (int i = 0; i < SopraProject.ObjectApi.Site.GetSitesCount(); i++)
                {
                    if (i != siteId)
                    {
                        var sites = new SopraProject.ObjectApi.Site(new SiteIdentifier(i.ToString()));
                        for (int j = 0; j < sites.Rooms.Count(); j++)
                        {
                            rooms.Remove(sites.Rooms[j]);
                        }
                        //rooms.RemoveAll(new SopraProject.ObjectApi.Site(new SiteIdentifier(i.ToString())).Rooms);
                    }
                }
                //rooms.RemoveAll(sites.Rooms);
            }
            if (personCount != -1)
            {
                for (int i = 0; i < rooms.Count(); i++)
                {
                    if (rooms[i].Capacity > personCount)
                    {
                        rooms.Remove(rooms[i]);
                    }
                }
            }
            if (particularities != null)
            {
                for (int i = 0; i < rooms.Count(); i++)
                {
                    //rooms.ElementAt(i).Particularities.Count();
                    // rooms[i].GetParticularities().Union(particularities);
                    var parts = rooms[i].Particularities.ToList().ConvertAll(p => p.Identifier);
                    if (parts.Union(particularities).Count() != rooms[i].Particularities.Count())
                    {
                        rooms.Remove(rooms.ElementAt(i));
                    }
                }
            }
            return Content(Tools.Serializer.Serialize(rooms));
        }


        [AuthorizationFilter]
        public ActionResult Sites()
        {
            var sites = SopraProject.ObjectApi.Site.GetAllSites();
            return Content(Tools.Serializer.Serialize(sites));
        }
    }
}

