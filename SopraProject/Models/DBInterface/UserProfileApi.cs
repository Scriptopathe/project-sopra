﻿using System;
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
        /// Returns true if the profile exists.
        /// </summary>
        /// <returns><c>true</c>, if the profile exists for the given username, <c>false</c> otherwise.</returns>
        /// <param name="username">Username.</param>
        private bool ProfileExists(UserIdentifier username)
        {
            bool exists = false;
            using (var ctx = new Database.MainContext())
            {
                var query = from profile in ctx.UsersProfile
                            where profile.Username.Equals(username.Value)
                            select profile;
                exists = query.Count() != 0;
            }
            return exists;
        }

        /// <summary>
        /// Make sure that the user profile corresponding to the given username exists.
        /// If it doesn't exist, it is created.
        /// </summary>
        /// <param name="username">Username.</param>
        private void ValidateProfile(UserIdentifier username)
        {
            if (!ProfileExists(username))
            {
                using (var ctx = new Database.MainContext())
                {
                    Database.MCUserProfile userProfile = new SopraProject.Database.MCUserProfile();
                    userProfile.Username = username.Value;
                    ctx.UsersProfile.Add(userProfile);
                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Gets the SiteIdentifier corresponding to this user location.
        /// </summary>
        /// <value>The location.</value>
        public SiteIdentifier GetLocation(UserIdentifier username)
        {
            ValidateProfile(username);

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
            ValidateProfile(username);

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

