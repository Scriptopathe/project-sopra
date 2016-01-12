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
            Configuration.Provider.Instance.Cache.CacheEnabled = true;
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Sets the CacheEnabled value of the Cache configuration.
        /// </summary>
        [HttpGet]
        [AuthorizationFilter(adminOnly: true)]
        public ActionResult CacheEnabled()
        {
            return Content(Configuration.Provider.Instance.Cache.CacheEnabled.ToString());
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