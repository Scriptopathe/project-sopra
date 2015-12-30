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
            var rooms = SopraProject.ObjectApi.Room.GetAllRooms();
            return Content(Tools.Serializer.Serialize(rooms));
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
			var rooms = Room.GetAllRooms();
			if (siteId != -1) {
				rooms = new List<Room> (Site.Get (new SiteIdentifier (siteId.ToString ())).Rooms);

				//rooms.AddRange(Site.Get (new SiteIdentifier (siteId.ToString ())).Rooms);
                
				//List<SopraProject.ObjectApi.Site> sites = null;
				/*for (int i = 0; i < SopraProject.ObjectApi.Site.GetSitesCount(); i++)
                {
                    if (i != siteId)
                    {
                        var sites = SopraProject.ObjectApi.Site.Get(new SiteIdentifier(i.ToString()));
                        for (int j = 0; j < sites.Rooms.Count(); j++)
                        {
                            rooms.Remove(sites.Rooms[j]);
                        }
                        //rooms.RemoveAll(new SopraProject.ObjectApi.Site(new SiteIdentifier(i.ToString())).Rooms);
                    }
                }
                //rooms.RemoveAll(sites.Rooms);*/
				
			} else {
				rooms = Room.GetAllRooms();
			}
            if (personCount != -1)
            {
				rooms = rooms.FindAll (room => room.Capacity >= personCount);
                /*for (int i = 0; i < rooms.Count(); i++)
                {
                    if (rooms[i].Capacity > personCount)
                    {
                        rooms.Remove(rooms[i]);
                    }
                }*/
            }
            if (particularities != null)
            {
				//Garder uniquement les salles qui ont le même nombre de particularités que celles demandées dans la recherche
				rooms = rooms.FindAll (room => room.Particularities.Count() == particularities.Length);
				Boolean find = false;

				for (int i = 0; i < rooms.Count(); i++) {
					var parts = rooms[i].Particularities.ToList ();
					for (int j = 0; j < particularities.Length; j++) {
						for (int k = 0; k < particularities.Length; k++){
							if (parts[j].Identifier.ToString() == particularities[k]){
								find = true;
								k = particularities.Length;
							}
						}
						if (find == false) {
							rooms.Remove(rooms.ElementAt(i));
							j = particularities.Length;
						}
						find = false;
					}
				}

				/*for (int i = 0; i < rooms.Count(); i++)
                {
                    //rooms.ElementAt(i).Particularities.Count();
                    // rooms[i].GetParticularities().Union(particularities);
                    var parts = rooms[i].Particularities.ToList().ConvertAll(p => p.Identifier);
                    if (parts.Union(particularities).Count() != rooms[i].Particularities.Count())
                    {
                        rooms.Remove(rooms.ElementAt(i));
                    }
                }*/
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
    }
}

