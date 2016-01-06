using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace SopraProject.Database
{
	/// <summary>
	/// The user profile is
	/// </summary>
	public class MCUserProfile
	{
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		[Key]
		public string Username { get; set; }
        /// <summary>
        /// Gets or sets the user site.
        /// </summary>
        /// <value>The user site.</value>
        public int SiteID { get; set; }
	}

    public class MCSite
	{
		/// <summary>
		/// Gets or sets the site ID.
		/// </summary>
		/// <value>The site ID.</value>
		[Key]
		public int SiteID { get; set; }
		/// <summary>
		/// Gets or sets the name of the site.
		/// </summary>
		/// <value>The name of the site.</value>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the site address.
		/// </summary>
		/// <value>The site address.</value>
		public string Address { get; set; }
		/// <summary>
		/// Gets or sets the rooms list.
		/// </summary>
		/// <value>The rooms list.</value>
        public virtual List<MCRoom> Rooms { get; set; }
	}

	/// <summary>
	/// Room.
	/// </summary>
    public class MCRoom
	{
		/// <summary>
		/// Gets or sets the room ID.
		/// </summary>
		/// <value>The room ID.</value>
		[Key]
		public int RoomID { get; set; }
		/// <summary>
		/// Gets or sets the capacity.
		/// </summary>
		/// <value>The capacity.</value>
		public int Capacity { get; set; }
		/// <summary>
		/// Gets or sets the name of the room.
		/// </summary>
		/// <value>The name of the room.</value>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the room particularities.
		/// </summary>
		/// <value>The room particularities.</value>
        public virtual List<MCParticularity> Particularities { get; set; }
	}

	/// <summary>
	/// Particularity.
	/// </summary>
    public class MCParticularity
	{
		/// <summary>
		/// Gets or sets the particularity ID.
		/// </summary>
		/// <value>The particularity ID.</value>
		[Key]
		public int ParticularityID { get; set; }
		/// <summary>
		/// Gets or sets the name of the particularity.
		/// </summary>
		/// <value>The name of the particularity.</value>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the particularity description.
		/// </summary>
		/// <value>The particularity description.</value>
		public string Description { get; set; }

        /// <summary>
        /// Many to many relationship with rooms.
        /// </summary>
        public virtual ICollection<MCRoom> Rooms { get; set; }
	}

	/// <summary>
	/// Main context.
	/// </summary>
	[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
	public class MainContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SopraProject.Database.MainContext"/> class.
		/// </summary>
		/// <param name="connexion">Connexion.</param>
        public MainContext () : base("mainContext")
		{
			System.Data.Entity.Database.SetInitializer<MainContext>(new CreateDatabaseIfNotExists<MainContext>());
		}
		/// <summary>
		/// Gets or sets the users profile.
		/// </summary>
		/// <value>The users profile.</value>
        public DbSet<MCUserProfile> UsersProfile { get; set; }
		/// <summary>
		/// Gets or sets the sites.
		/// </summary>
		/// <value>The sites.</value>
        public DbSet<MCSite> Sites  { get; set; }
		/// <summary>
		/// Gets or sets the rooms.
		/// </summary>
		/// <value>The rooms.</value>
        public DbSet<MCRoom> Rooms { get; set; }
		/// <summary>
		/// Gets or sets the particularities.
		/// </summary>
		/// <value>The particularities.</value>
        public DbSet<MCParticularity> Particularities  { get; set; }
	}
}
	