using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.Configuration
{
    public delegate void ConfStateChangedDelegate();
    /// <summary>
    /// Singleton instance containing all the configuration modules.
    /// </summary>
    public class Provider
    {
        #region Const
        private readonly string SearchConfPath = "conf-search.xml";
        private readonly string CacheConfPath = "conf-search.xml";
        #endregion

        #region Static
        private static Provider s_instance;

        /// <summary>
        /// Gets the ConfigurationProvider singleton instance.
        /// </summary>
        public static Provider Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new Provider();
                return s_instance;
            }
        }
        #endregion

        /// <summary>
        /// Gets the CacheConfiguration instance.
        /// </summary>
        public CacheConfiguration Cache
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the SearchConfiguration instance.
        /// </summary>
        public SearchConfiguration Search
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the Configuration provider.
        /// </summary>
        private Provider()
        {
            Cache = new CacheConfiguration();
            Search = new SearchConfiguration();

            Cache.StateChanged += Cache_StateChanged;
            Search.StateChanged += Search_StateChanged;

            LoadConf();
        }

        /// <summary>
        /// Loads the configuration from disk.
        /// </summary>
        private void LoadConf()
        {
            // Works even when the file does not exist !
            Cache = Tools.Serializer.Deserialize<CacheConfiguration>(CacheConfPath);
            Search = Tools.Serializer.Deserialize<SearchConfiguration>(SearchConfPath);
        }

        private void Search_StateChanged()
        {
            System.IO.File.WriteAllText(SearchConfPath, Tools.Serializer.Serialize(Search));
        }

        private void Cache_StateChanged()
        {
            System.IO.File.WriteAllText(CacheConfPath, Tools.Serializer.Serialize(Cache));
        }
        
    }
}