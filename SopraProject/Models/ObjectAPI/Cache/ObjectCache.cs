using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Web;

namespace SopraProject.ObjectApi.Cache
{

    /// <summary>
    /// Represents a generic object cache.
    /// </summary>
    public class ObjectCache<IdentifierT, T> where T : new()
    {
        /// <summary>
        /// This constant determines if the cache is enabled.
        /// 
        /// If it is enabled, the object retrieved from the database will be kept in memory.
        /// If enabled then :
        ///     - This will increase memory usage.
        ///     - This will HUGELY increase performance.
        /// Note that if caching is enabled EXTERNAL MODIFICATION WILL LEAD TO INCONSISTANT SERVER STATE UNTIL REBOOT.
        /// </summary>
        public const bool CacheEnabled = false;
        
        /// <summary>
        /// Cache policy used to perform cacheing.
        /// 
        /// There can be different cache policies which have their own memory/UC tradeoffs.
        /// </summary>
        ICachePolicy<IdentifierT, T> _cache = new FastCachePolicy<IdentifierT, T>();

        /// <summary>
        /// Creates a new instance of ObjectCache.
        /// </summary>
        public ObjectCache() { }

        /// <summary>
        /// Gets an object from the cache if it already exists.
        /// Otherwise, creates a new instance.
        /// </summary>
        public T Get(Identifier<IdentifierT> identifier)
        {
            lock(_cache)
            {
                if (CacheEnabled && _cache.Contains(identifier.Value))
                {
                    return _cache[identifier.Value];
                }
                else
                {
                    var item = Construct(new Type[] { identifier.GetType() }, new object[] { identifier });

                    if (CacheEnabled)
                        _cache.Add(identifier.Value, item);

                    return item;
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the given type with the given parameters types and value.
        /// This method can invoke private constructors.
        /// </summary>
        /// <typeparam name="T">Type to instantiate</typeparam>
        /// <param name="paramTypes">Type of the parameters in the signature of the constructor</param>
        /// <param name="paramValues">Values of the parameters to send to the constructor.</param>
        private static T Construct(Type[] paramTypes, object[] paramValues)
        {
            Type t = typeof(T);

            ConstructorInfo ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, paramTypes, null);

            return (T)ci.Invoke(paramValues);
        }
    }
}