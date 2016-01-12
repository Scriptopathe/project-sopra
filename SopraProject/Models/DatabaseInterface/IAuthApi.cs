using System;
using SopraProject.Models.Identifiers;

namespace SopraProject.Models.DatabaseInterface
{
    /// <summary>
    /// This interface describes the messages needed to authenticate an user
    /// on various authentication systems (LDAP, Exchange).
    /// </summary>
    public interface IAuthApi
    {
        /// <summary>
        /// Authenticate the User with specified username and password.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        bool Authenticate(UserIdentifier username, string password);
    }
}

