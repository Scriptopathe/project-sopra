using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.ObjectApi
{
    /// <summary>
    /// Represents a generic object cache.
    /// </summary>
    public class ObjectCache<T, IdentifierT> where T : new()
    {
        /// <summary>
        /// This constant determines if the cache is enabled.
        /// 
        /// If it is enabled, the object retrieved from the database will be kept in memory.
        /// If enabled then :
        ///     - This will increase memory usage.
        ///     - This will increase performance.
        /// Note that if caching is enabled EXTERNAL MODIFICATION WILL LEAD TO INCONSISTANT SERVER STATE UNTIL REBOOT.
        /// </summary>
        public const bool CacheEnabled = true;

        Dictionary<IdentifierT, T> _cache;

        /// <summary>
        /// Creates a new instance of ObjectCache.
        /// </summary>
        public ObjectCache() { _cache = new Dictionary<IdentifierT, T>(); }

        /// <summary>
        /// Gets an object from the cache if it already exists.
        /// Otherwise, creates a new instance.
        /// </summary>
        public T Get(Identifier<IdentifierT> identifier)
        {
            if(CacheEnabled && _cache.ContainsKey(identifier.Value))
            {
                return _cache[identifier.Value];
            }
            else
            {
                // todo 
                return (T)Activator.CreateInstance(typeof(T), identifier, );
            }
        }

    }
}