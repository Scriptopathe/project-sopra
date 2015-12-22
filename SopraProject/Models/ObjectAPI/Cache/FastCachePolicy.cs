using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.ObjectApi.Cache
{
    /// <summary>
    /// Represents a Fast Cache Policy.
    /// 
    /// This cache policy is the fastest because it includes no overhead.
    /// However it doesn't feature any memory limit or regulation technique.
    /// </summary>
    public class FastCachePolicy<IdentifierT, T> : ICachePolicy<IdentifierT, T>
    {
        Dictionary<IdentifierT, T> m_cache;
        public FastCachePolicy() { m_cache = new Dictionary<IdentifierT, T>(); }

        public void Add(IdentifierT identifier, T value)
        {
            m_cache.Add(identifier, value);
        }

        public bool Contains(IdentifierT identifier)
        {
            return m_cache.ContainsKey(identifier);
        }


        public T this[IdentifierT identifier]
        {
            get { return m_cache[identifier]; }
            set { m_cache[identifier] = value; }
        }
    }
}