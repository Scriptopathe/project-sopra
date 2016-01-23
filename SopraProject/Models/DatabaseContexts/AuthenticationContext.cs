using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace SopraProject.Models.DatabaseContexts
{
	/// <summary>
	/// User.
	/// </summary>
	public class ACUser
	{
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		[Key]
		public string Username { get; set; }
		/// <summary>
		/// Gets or sets the user password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }
	}

    /// <summary>
    /// Authentication context.
    /// </summary>
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AuthenticationContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SopraProject.Database.AuthenticationContext"/> class.
		/// </summary>
		public AuthenticationContext () : base("authContext")
		{
#if DEBUG
            System.Data.Entity.Database.SetInitializer<AuthenticationContext>(new CreateDatabaseIfNotExists<AuthenticationContext>());
#else
            // Azure cloud conf
            if (MainContext.ALWAYS_DROP)
                System.Data.Entity.Database.SetInitializer<AuthenticationContext>(new DropCreateDatabaseAlways<AuthenticationContext>());
            else
                System.Data.Entity.Database.SetInitializer<AuthenticationContext>(new CreateDatabaseIfNotExists<AuthenticationContext>());
#endif
        }
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<ACUser> Users { get; set; }
	}
}

