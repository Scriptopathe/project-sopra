using System;
using System.Linq;
using System.Collections.Generic;

namespace SopraProject
{
    public class UserProfileApi : IUserProfileAPI
    {
        public UserProfileApi()
        {
            
        }
        /// <summary>
        /// Gets the SiteIdentifier corresponding to this user location.
        /// </summary>
        /// <value>The location.</value>
        public SiteIdentifier GetLocation(UserIdentifier username)
        {
            SiteIdentifier val;
            using (var ctx = new Database.MainContext())
            {
                var query = from profile in ctx.UsersProfile
                            where profile.Username.Equals(username.Value)
                            select profile.SiteID;

                val = new SiteIdentifier(query.First().ToString());
            }
            return val;
        }
        /// <summary>
        /// Sets the given user location.
        /// When setting, if the user does not yet exist in the underlaying storage system, it is created.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="siteId">Site identifier.</param>
        public void SetLocation(UserIdentifier username, SiteIdentifier siteId)
        {
            using (var ctx = new Database.MainContext())
            {
                var query = from profile in ctx.UsersProfile
                            where profile.Username.Equals(username.Value)
                            select profile;

                Database.MCUserProfile prof = query.First();
                prof.SiteID = Int32.Parse(siteId.Value);
                ctx.SaveChanges();
            }
        }
    }
}

