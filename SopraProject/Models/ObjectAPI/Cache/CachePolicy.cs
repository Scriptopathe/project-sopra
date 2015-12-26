using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SopraProject.ObjectApi.Cache
{
    /// <summary>
    /// Describes a cache policy interface.
    /// </summary>
    public interface ICachePolicy<IdentifierT, T>
    {
        /// <summary>
        /// Returns a value indicating if the cache contains the value with the given identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the value to test.</param>
        bool Contains(IdentifierT identifier);

        /// <summary>
        /// Adds a given identifier / value pair to the cache.
        /// </summary>
        /// <param name="identifier">Identifier of the value to store in the cache.</param>
        /// <param name="value">Value to store in the cache.</param>
        void Add(IdentifierT identifier, T value);

        /// <summary>
        /// Gets or sets the value associated to the given identifier in the cache.
        /// </summary>
        T this[IdentifierT identifier] { get; set; }
    }
}