using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SopraProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;
            Random rd = new Random();

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = "NET";

            MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(
                "Server=myServerAddress;" +
                "Port=1234;" +
                "Database=myDataBase;" +
                "Uid=myUsername;\n" +
                "Pwd=myPassword;");
            

            using (var ctx = new DBTestContext2(con))
            {
                Machin m = new Machin() { UnMachin = "Hhaha" };

                for(int i = 0; i < rd.Next(10); i++)
                    m.DesTrucs.Add(new Truc() { ID = rd.Next(500000) });

     
                ctx.Machins.Add(m);

                ctx.SaveChanges();

                Response.Write("LA BDD contient ");

                var query = from b in ctx.Machins
                                        select b;
                foreach (var item in query)
                {
                    Response.Write(item.UnMachin);
                    foreach (var item2 in item.DesTrucs)
                    {
                        Response.Write(" truc=" + item2);
                    }
                }
            }

            return View();
        }
    }
}