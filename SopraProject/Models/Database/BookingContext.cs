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
        public int RoomID { get; set; }
        /// <summary>
        /// Gets or sets the contacts' e-mail addresses.
        /// </summary>
        /// <value>The list of contacts.</value>
        public List<string> Contacts { get; set; }
        /// <summary>
        /// Gets or sets the booking date.
        /// </summary>
        /// <value>The booking date.</value>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the booking end date.
        /// </summary>
        /// <value>The booking end date.</value>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Gets or sets the person count.
        /// </summary>
        /// <value>The person count.</value>
        public int PersonCount { get; set; }
        /// <summary>
        /// Gets or sets the booking subject.
        /// </summary>
        /// <value>The booking subject.</value>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the number of participants of this booking.
        /// </summary>
        public int ParticipantsCount { get; set; }
    }

    /// <summary>
    /// Booking context.
    /// </summary>
    public class BookingContext : DbContext
    {
        /// <summary>
        /// Gets or sets the bookings.
        /// </summary>
        /// <value>The bookings.</value>
        public DbSet<BCBooking> Bookings { get; set; }



        public BookingContext() : base("bookingContext")
		{
			Database.SetInitializer<BookingContext>(new CreateDatabaseIfNotExists<BookingContext>());
        }
    }
}

