using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace SopraProject.Database
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
		public string UserName { get; set; }
		/// <summary>
		/// Gets or sets the user password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }
	}

	/// <summary>
	/// Authentication context.
	/// </summary>
	public class AuthenticationContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SopraProject.Database.AuthenticationContext"/> class.
		/// </summary>
		public AuthenticationContext (string connexion) : base(connexion)
		{
		}
		/// <summary>
		/// Gets or sets the users.
		/// </summary>
		/// <value>The users.</value>
		public DbSet<ACUser> Users { get; set; }
	}
}

