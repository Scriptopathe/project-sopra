using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
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

        public ActionResult Rooms()
        {
            //var rooms = SopraProject.ObjectApi.Room.GetAllRooms();
            // Response.Write(Serializer.Serialize<List<ObjectApi.Room>>(rooms));

            return Content(Serializer.Serialize<Test>(new Test() { Str = "hahaha", Truc = "kokoko" }));
        }
    }
}

