using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace SopraProject.Database
{
    /// <summary>
    /// Booking.
    /// </summary>
    public class BCBooking 
    {
        /// <summary>
        /// Gets or sets the booking ID.
        /// </summary>
        /// <value>The booking ID.</value>
        [Key]
        public int BookingID { get; set; }
        /// <summary>
        /// Gets or sets the room ID.
        /// </summary>
        /// <value>The room ID.</value>
        public BCRoom RoomID { get; set; }
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public BCUserProfile BookingUser { get; set; }
        /// <summary>
        /// Gets or sets the booking date.
        /// </summary>
        /// <value>The booking date.</value>
        public DateTime BookingDate { get; set; }
        /// <summary>
        /// Gets or sets the person count.
        /// </summary>
        /// <value>The person count.</value>
        public int PersonCount { get; set; }
        /// <summary>
        /// Gets or sets the booking subject.
        /// </summary>
        /// <value>The booking subject.</value>
        public string BookingSubject { get; set; }
    }

    /// <summary>
    /// The user profile is
    /// </summary>
    public class BCUserProfile
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [Key]
        public string UserName { get; set; }
    }

    /// <summary>
    /// Room.
    /// </summary>
    public class BCRoom
    {
        /// <summary>
        /// Gets or sets the room ID.
        /// </summary>
        /// <value>The room ID.</value>
        [Key]
        public int RoomID { get; set; }
    }

    /// <summary>
    /// Booking context.
    /// </summary>
    public class BookingContext : DbContext
    {
        /// <summary>
        /// Gets or sets the users profile.
        /// </summary>
        /// <value>The users profile.</value>
        public DbSet<BCUserProfile> UsersProfile { get; set; }
        /// <summary>
        /// Gets or sets the rooms.
        /// </summary>
        /// <value>The rooms.</value>
        public DbSet<BCRoom> Rooms { get; set; }
        /// <summary>
        /// Gets or sets the bookings.
        /// </summary>
        /// <value>The bookings.</value>
        public DbSet<BCBooking> Bookings { get; set; }



        public BookingContext() : base("bookingContext")
        {
        }
    }
}

