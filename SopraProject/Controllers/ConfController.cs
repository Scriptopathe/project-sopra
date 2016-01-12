using System.Web.Mvc;

namespace SopraProject.Controllers
{
    /// <summary>
    /// Controller containing all the configuration functions.
    /// </summary>
    public class ConfController : BaseController
    {
        /// <summary>
        /// Sets the CacheEnabled value of the Cache configuration.
        /// </summary>
        [HttpPost]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult CacheEnabled(bool value)
        {
            Configuration.Provider.Instance.Cache.CacheEnabled = value;
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the CacheEnabled value of the Cache configuration.
        /// </summary>
        [HttpGet]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult CacheEnabled()
        {
            return Content(Configuration.Provider.Instance.Cache.CacheEnabled.ToString());
        }

        /// <summary>
        /// Sets the DayStart value of the Cache configuration.
        /// </summary>
        [HttpPost]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult SearchDayStart(int dayStart)
        {
            try
            {
                Checked(() => CheckRange(dayStart, 1, 23), "dayStart");
            }
            catch (ParameterCheckException e)
            {
                return Error(e.Message);
            }

            Configuration.Provider.Instance.Search.DayStart = dayStart;
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the DayStart value of the Search configuration.
        /// </summary>
        [HttpGet]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult SearchDayStart()
        {
            return Content(Configuration.Provider.Instance.Search.DayStart.ToString());
        }


        /// <summary>
        /// Sets the DayEnd value of the Cache configuration.
        /// </summary>
        [HttpPost]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult SearchDayEnd(int dayEnd)
        {
            try
            {
                Checked(() => CheckRange(dayEnd, 1, 23), "dayEnd");
            }
            catch (ParameterCheckException e)
            {
                return Error(e.Message);
            }

            Configuration.Provider.Instance.Search.DayEnd = dayEnd;
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the DayEnd value of the Search configuration.
        /// </summary>
        [HttpGet]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult SearchDayEnd()
        {
            return Content(Configuration.Provider.Instance.Search.DayEnd.ToString());
        }

        /// <summary>
        /// Invalidates all the caches.
        /// </summary>
        [HttpGet]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult InvalidateCache()
        {
            SopraProject.Models.ObjectApi.Cache.ObjectCacheCollector.InvalidateAll();
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }


    }
}