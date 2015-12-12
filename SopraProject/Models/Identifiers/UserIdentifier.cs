using System;

namespace SopraProject
{
    public class UserIdentifier : Identifier<string>
    {
        public UserIdentifier(string id) : base(id) { }

        /// <summary>
        /// Implicit conversion from string to UserIdentifier for more concise
        /// syntax.
        /// </summary>
        /// <param name="username">Username.</param>
        public static implicit operator UserIdentifier(string username)
        {
            return new UserIdentifier(username);
        }
    }
}

