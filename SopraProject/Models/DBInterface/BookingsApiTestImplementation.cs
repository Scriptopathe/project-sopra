﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace SopraProject
{
    public class BookingsApiTestImplementation : IBookingsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.BookingsApiTestImplementation"/> class.
        /// </summary>
        public BookingsApiTestImplementation()
        {
            
        }
        /// <summary>
        /// Gets a list containing every booking between the given startDate and endDate.
        /// The bookings which are not totally covered by the given time period (startDate -> endDate)
        /// are also included.
        /// The bookings are represented by their id.
        /// </summary>
        /// <returns>The bookings.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public List<BookingIdentifier> GetBookings(RoomIdentifier identifier, DateTime startDate, DateTime endDate)
        {
            List<BookingIdentifier> val;
            using (var ctx = new Database.BookingContext())
            {
                var query = from booking in ctx.Bookings
                            where booking.EndDate > startDate && booking.StartDate < endDate
                            select booking.BookingID;
                
                val = query.ToList().ConvertAll(id => new BookingIdentifier(id.ToString()));
            }
            return val;
        }
        /// <summary>
        /// Gets the booking subject.
        /// </summary>
        /// <returns>The booking subject.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        public string GetBookingSubject(BookingIdentifier bookingId)
        {
            string val;
            using (var ctx = new Database.BookingContext())
            {
                var query = from booking in ctx.Bookings
                            where booking.BookingID.ToString().Equals(bookingId.Value)
                            select booking.Subject;
                val = query.First();
            }
            return val;
        }
        /// <summary>
        /// Gets the booking room.
        /// </summary>
        /// <returns>The booking room.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        public RoomIdentifier GetBookingRoom(BookingIdentifier bookingId)
        {
            RoomIdentifier val;
            using (var ctx = new Database.BookingContext())
            {
                var query = from booking in ctx.Bookings
                            where booking.BookingID.ToString().Equals(bookingId.Value)
                            select booking.RoomID;
                val = new RoomIdentifier(query.First().ToString());
            }
            return val;
        }
        /// <summary>
        /// Gets the booking start date.
        /// </summary>
        /// <returns>The booking start date.</returns>
        /// <param name="reversationId">Reversation identifier.</param>
        public DateTime GetBookingStartDate(BookingIdentifier bookingId)
        {
            DateTime val;
            using (var ctx = new Database.BookingContext())
            {
                var query = from booking in ctx.Bookings
                            where booking.BookingID.ToString().Equals(bookingId.Value)
                            select booking.StartDate;
                val = query.First();
            }
            return val;
        }
        /// <summary>
        /// Gets the booking end date.
        /// </summary>
        /// <returns>The booking end date.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        public DateTime GetBookingEndDate(BookingIdentifier bookingId)
        {
            DateTime val;
            using (var ctx = new Database.BookingContext())
            {
                var query = from booking in ctx.Bookings
                            where booking.BookingID.ToString().Equals(bookingId.Value)
                            select booking.EndDate;
                val = query.First();
            }
            return val;
        }
        /// <summary>
        /// Gets the booking contacts.
        /// </summary>
        /// <returns>The booking contacts.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        public List<string> GetBookingContacts(BookingIdentifier bookingId)
        {
            List<string> val;
            using (var ctx = new Database.BookingContext())
            {
                var query = from booking in ctx.Bookings
                            where booking.BookingID.ToString().Equals(bookingId.Value)
                            select booking.Contacts;
                val = query.First();
            }
            return val;
        }
    }
}
