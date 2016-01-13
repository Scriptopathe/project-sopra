using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SopraProject.Tools.Extensions.Date;
using System.Xml.Serialization;
using System.Web.Security;
using SopraProject.Models.ObjectApi;
using SopraProject.Models.Identifiers;
using SopraProject.Models.DatabaseContexts;

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

            User usr = Models.ObjectApi.User.Authenticate(
                new UserIdentifier(username), 
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
            DatabaseWorker.CreateDatabase();
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
            return Content(Tools.Serializer.Serialize(user));
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
            user.Location = Site.Get(new SiteIdentifier(siteId.ToString()));
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
            //List<Tuple<string, List<Room>>> siteRooms = new List<Tuple<string, List<Room>>>();
            List<SiteWithRooms> siteRooms = new List<SiteWithRooms>();
            var sites = Site.GetAllSites();
            foreach(Site site in sites)
            { 
                siteRooms.Add(new SiteWithRooms(site.Identifier.Value));
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
            var room = Room.Get(new RoomIdentifier(roomId.ToString()));
            return Content(Tools.Serializer.Serialize(room));
        }

		/// <summary>
		/// Gets the list of rooms located at the given site.
		/// </summary>
		/// <returns>The by site.</returns>
		/// <param name="siteId">Site identifier.</param>
		[HttpGet]
		[AuthorizationFilter]
		public ActionResult RoomsBySite(string siteId)
		{
			var site = Site.Get(new SiteIdentifier(siteId));
			var rooms = site.Rooms;
			return Content(Tools.Serializer.Serialize(rooms));
		}

        [HttpGet]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult Report(string startDate, string endDate, string roomId)
        {
            try
            {
                var sDate = Checked(() => startDate.DeserializeDate(), "startDate");
                var eDate = Checked(() => endDate.DeserializeDate(), "endDate");
                Checked(() => CheckIsPositive(Int32.Parse(roomId)), "roomId");

                var report = new UsageReport(Room.Get(new RoomIdentifier(roomId)), sDate, eDate);
                return Content(Tools.Serializer.Serialize(report));
            }
            catch (ParameterCheckException e)
            {
                return Error(e.Message);
            }
        }

        /// <summary>
        /// Gets all sites as a list in XML format.
        /// </summary>
        [AuthorizationFilter]
        public ActionResult Sites()
        {
            var sites = Site.GetAllSites();
            return Content(Tools.Serializer.Serialize(sites));
        }

		//Requête qui renvoie toutes les particularités avec nom et id
		[AuthorizationFilter]
		public ActionResult Particularities()
		{
            List<Particularity> particularities = Particularity.GetAllParticularities();
			return Content(Tools.Serializer.Serialize(particularities));
		}



        [HttpGet]
        [AuthorizationFilter]
        /// <summary>
        /// Searchs the with date.
        /// </summary>
        /// <returns>The with date.</returns>
        /// <param name="siteId">Site identifier.</param>
        /// <param name="personCount">Person count.</param>
        /// <param name="particularities">Particularities.</param>
        /// <param name="startDate">Start date. Format MM/DD/YYYY-HH:MM:SS</param>
        /// <param name="endDate">End date.</param>
        public ActionResult Search(int siteId = -1, int personCount = -1, int meetingDuration = 15, string[] particularities = null, string startDate = null, string endDate = null)
        {
            try
            {
                if (particularities == null || (particularities.Length == 1 && particularities[0] == String.Empty))
                    particularities = new string[0];

                Checked(() => CheckIsPositive(siteId), "siteId", "The specified site is not valid !");
                Checked(() => CheckIsPositive(personCount), "personCount", "The number of people must be equal or greater than 0");
                var sDate = Checked(() => startDate.DeserializeDateTime(), "startDate");
                var eDate = Checked(() => endDate.DeserializeDateTime(), "endDate");

                ResearchAlgorithm ra = new ResearchAlgorithm();
                List<ResearchAlgorithm.RoomSearchResult> result = ra.Search(siteId, personCount, particularities, meetingDuration, sDate, eDate);
                return Content(Tools.Serializer.Serialize(result));
            }
            catch (ParameterCheckException e)
            {
                return Error(e.Message);
            }
        }

    }


}

