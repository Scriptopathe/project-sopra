using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.Configuration
{
    /// <summary>
    /// Contains all the cache configuration values.
    /// </summary>
    public class CacheConfiguration
    {
        public event ConfStateChangedDelegate StateChanged;
        private bool _cacheEnabled = true;

        /// <summary>
        /// This constant determines if the cache is enabled.
        /// 
        /// If it is enabled, the object retrieved from the database will be kept in memory.
        /// If enabled then :
        ///     - This will increase memory usage.
        ///     - This will HUGELY increase performance.
        /// Note that if caching is enabled EXTERNAL MODIFICATION WILL LEAD TO INCONSISTANT SERVER STATE UNTIL REBOOT.
        /// </summary>
        public bool CacheEnabled
        {
            get { return _cacheEnabled; }
            set
            {
                _cacheEnabled = value;
                if(StateChanged != null)
                    StateChanged();
            }
        }

        public CacheConfiguration() { }
    }
}