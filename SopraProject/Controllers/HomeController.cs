using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace SopraProject.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;
            Random rd = new Random();

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = "NET";
            Database.DatabaseWorker.CreateDatabase();
            /*string connectionString = 
                "Server=localhost;" +
                "Port=3306;" +
                "Database=sopra;" +
                "Uid=sopra;\n" +
                "Pwd=sopra;";*/
            /*MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            con.Open();*/

            /*using (var ctx = new DBTestContext2("mainContext"))
            {
                Machin m = new Machin() { UnMachin = "Hhaha" };

                for(int i = 0; i < 2+rd.Next(10); i++)
                    m.DesTrucs.Add(new Truc() { ID = rd.Next(500000) });

     
                ctx.Machins.Add(m);

                ctx.SaveChanges();

                Response.Write("LA BDD contient ");

                var query = from b in ctx.Machins
                            select b;
                
                foreach (var item in query.ToList())
                {
                    Response.Write(item.UnMachin);
                    int a;
                    if (item.DesTrucs.Count != 0)
                    {
                        a = 0;
                    }
                    else
                    {
                        a = 5;
                    }
                    foreach (var item2 in item.DesTrucs)
                    {
                        Response.Write(" truc=" + item2.ID);
                    }
                }
            }*/

            return View();
        }
    }
}