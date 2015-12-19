using System;

namespace SopraProject
{
    /// <summary>
    /// This interface describes the messages used to access to the users' application
    /// profile.
    /// 
    /// The underlaying Database must maintain consistent state about user names in order
    /// to generate valid identifiers linked to the authentication system.
    /// </summary>
    public interface IUserProfileAPI
    {
        /// <summary>
        /// Gets the SiteIdentifier corresponding to this user location.
        /// </summary>
        /// <value>The location.</value>
        SiteIdentifier GetLocation(UserIdentifier username);
        /// <summary>
        /// Sets the given user location.
        /// When setting, if the user does not yet exist in the underlaying storage system, it is created.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="siteId">Site identifier.</param>
        void SetLocation(UserIdentifier username, SiteIdentifier siteId);
    }
}

