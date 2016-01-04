using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SopraProject.Tools.Extensions.Date;
using System.Xml.Serialization;
using System.Web.Security;
using SopraProject.ObjectApi;

namespace SopraProject.Controllers
{
    /// <summary>
    /// API controler.
    /// </summary>
    public class ApiController : BaseController
    {
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

        /// <summary>
        /// Sets the user default location to the given site.
        /// </summary>
        /// <param name="siteId">The id of the site</param>
        [HttpPost]
        [AuthorizationFilter]
        public ActionResult Location(string siteId)
        {
            var user = GetUser();
            user.Location = ObjectApi.Site.Get(new SiteIdentifier(siteId.ToString()));
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the user's default location.
        /// </summary>
        /// <returns>The user's default location (site) identifier. For instance : 6</returns>
        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Location()
        {
            var user = GetUser();
            return Content(user.Location.Identifier.Value);
        }

        /// <summary>
        /// Gets all rooms as a list in XML format.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Rooms()
        {
            List<Tuple<string, List<Room>>> siteRooms = new List<Tuple<string, List<Room>>>();
            var sites = SopraProject.ObjectApi.Site.GetAllSites();
            foreach(Site site in sites)
            { 
                siteRooms.Add(Tuple.Create<string, List<Room>>(site.Identifier.Value, new List<Room>(Site.Get(new SiteIdentifier(site.Identifier.Value)).Rooms)));
            }
            return Content(Tools.Serializer.Serialize(siteRooms));
        }

        /// <summary>
        /// Gets the room with the given id in XML format.
        /// </summary>
        /// <param name="roomId">the room id</param>
        /// <returns>The room data in XML format</returns>
        [HttpGet]
        [AuthorizationFilter]
        public ActionResult GetRoomById(int roomId)
        {
            var room = ObjectApi.Room.Get(new RoomIdentifier(roomId.ToString()));
            return Content(Tools.Serializer.Serialize(room));
        }

        /// <summary>
        /// Gets the list of rooms that satifisfy the given requirements in XML format.
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="personCount"></param>
        /// <param name="particularities"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizationFilter]
		public ActionResult Search(int siteId=-1, int personCount=-1, string[] particularities=null)
        {
			List<Room> rooms = null;
			if (siteId != -1) {
				rooms = new List<Room> (Site.Get (new SiteIdentifier (siteId.ToString ())).Rooms);
				
			} else {
				rooms = Room.GetAllRooms();
			}
            if (personCount != -1)
            {
				rooms = rooms.FindAll (room => room.Capacity >= personCount);
            }
            if (particularities != null)
            {
				List<Room> filteredRooms = new List<Room> ();
				foreach(Room room in rooms)
                {
					var parts = room.Particularities.ToList().ConvertAll(p => p.Identifier.Value);
					var intersection = parts.Intersect (particularities);
					if (intersection.Count () == particularities.Count ()) {
						filteredRooms.Add (room);
					}
                }
				rooms = filteredRooms;
            }
            return Content(Tools.Serializer.Serialize(rooms));
        }

        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Report(string startDate, string endDate, string roomId)
        {
            // var report = new ObjectApi.UsageReport(new ObjectApi.Room(new RoomIdentifier(roomId)), DateTime.Parse(startDate), DateTime.Parse(endDate));
            // var report = new ObjectApi.UsageReport(new ObjectApi.Room(new RoomIdentifier(roomId)), new DateTime(2015, 11, 1), new DateTime(2015, 11, 30));
            var report = new ObjectApi.UsageReport(ObjectApi.Room.Get(new RoomIdentifier(roomId)), startDate.DeserializeDate(), endDate.DeserializeDate());
            return Content(Tools.Serializer.Serialize(report));
        }

        /// <summary>
        /// Gets all sites as a list in XML format.
        /// </summary>
        [AuthorizationFilter]
        public ActionResult Sites()
        {
            var sites = SopraProject.ObjectApi.Site.GetAllSites();
            return Content(Tools.Serializer.Serialize(sites));
        }

        [HttpGet]
        [AuthorizationFilter]
        public ActionResult Search(int siteId = -1, int personCount=-1, string[] particularities=null, DateTime? startDate = null, DateTime? endDate = null)
        {
            ResearchAlgorithm ra = new ResearchAlgorithm();
            var result = ra.research(siteId, personCount, particularities, startDate, endDate);
            return Content(Tools.Serializer.Serialize(result));
        }

    }
}

