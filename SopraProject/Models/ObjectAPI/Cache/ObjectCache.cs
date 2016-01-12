using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Web;
using SopraProject.Models.Identifiers;

namespace SopraProject.Models.ObjectApi.Cache
{
    /// <summary>
    /// Collects all instance of invalidable caches.
    /// </summary>
    public static class ObjectCacheCollector
    {
        private static List<IInvalidable> _caches = new List<IInvalidable>();
        public static void Add(IInvalidable cache) { _caches.Add(cache); }
        public static void InvalidateAll() { _caches.ForEach(cache => cache.Invalidate()); }
    }

    public interface IInvalidable
    {
        void Invalidate();
    }

    /// <summary>
    /// Represents a generic object cache.
    /// </summary>
    public class ObjectCache<IdentifierT, T> : IInvalidable where T : new()
    {       
        /// <summary>
        /// Cache policy used to perform cacheing.
        /// 
        /// There can be different cache policies which have their own memory/UC tradeoffs.
        /// </summary>
        ICachePolicy<IdentifierT, T> _cache = new FastCachePolicy<IdentifierT, T>();

        /// <summary>
        /// Creates a new instance of ObjectCache.
        /// </summary>
        public ObjectCache() { ObjectCacheCollector.Add(this); }

        /// <summary>
        /// Gets an object from the cache if it already exists.
        /// Otherwise, creates a new instance.
        /// 
        /// This operation is thread safe.
        /// </summary>
        public T Get(Identifier<IdentifierT> identifier)
        {
            bool cacheEnabled = Configuration.Provider.Instance.Cache.CacheEnabled;
            lock(_cache)
            {
                if (cacheEnabled && _cache.Contains(identifier.Value))
                {
                    return _cache[identifier.Value];
                }
                else
                {
                    var item = Construct(new Type[] { identifier.GetType() }, new object[] { identifier });

                    if (cacheEnabled)
                        _cache.Add(identifier.Value, item);

                    return item;
                }
            }
        }

        /// <summary>
        /// Invalidates all the objects in the cache.
        /// This operation clears all the cached data from the cache.
        /// 
        /// This operation is thread safe.
        /// </summary>
        public void Invalidate()
        {
            lock(_cache)
            {
                _cache.Invalidate();
            }
        }

        /// <summary>
        /// Invalidates the object with the given identifier.
        /// The next attempt to get that object will result to a cache miss and reload the object.
        /// 
        /// This operation is thread safe.
        /// </summary>
        /// <param name="identifier">Identifier of the object to invalidate.</param>
        public void Invalidate(IdentifierT identifier)
        {
            lock(_cache)
            {
                _cache.Invalidate(identifier);
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