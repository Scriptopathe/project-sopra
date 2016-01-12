using System;

namespace SopraProject.Models.Identifiers
{
    /// <summary>
    /// Represents an abstract identifier.
    /// This type is used to ensure the validity of identifiers at compile time.
    /// 
    /// Each identifier instance holds a value that can be used to identify an object
    /// in a database.
    /// </summary>
    public class Identifier<T>
    {
        /// <summary>
        /// Gets the internal value of the identifier.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; private set; }

        /// <summary>
        /// Creates a new identifier object given a value.
        /// </summary>
        public Identifier(T value)
        {
            this.Value = value;
        }
    }
}

