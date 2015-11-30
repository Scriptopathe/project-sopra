using System;
using System.Collections.Generic;
namespace SopraProject
{
    /// <summary>
    /// This interface describes the messages that can be send to get informations
    /// about bookings.
    /// This information is needed to perform room searching with the availability constraint.
    /// 
    /// The final implementation of this will be to look up for the bookings in the 
    /// outlook database.
    /// </summary>
    public interface IBookingsApi
    {
        /// <summary>
        /// Gets a list containing every booking between the given startDate and endDate.
        /// The bookings which are not totally covered by the given time period (startDate -> endDate)
        /// are also included.
        /// The bookings are represented by their id.
        /// </summary>
        /// <returns>The bookings.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        List<BookingIdentifier> GetBookings(RoomIdentifier identifier, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets the booking subject.
        /// </summary>
        /// <returns>The booking subject.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        string GetBookingSubject(BookingIdentifier bookingId);
        /// <summary>
        /// Gets the booking room.
        /// </summary>
        /// <returns>The booking room.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        string GetBookingRoom(BookingIdentifier bookingId);
        /// <summary>
        /// Gets the booking start date.
        /// </summary>
        /// <returns>The booking start date.</returns>
        /// <param name="reversationId">Reversation identifier.</param>
        DateTime GetBookingStartDate(BookingIdentifier reversationId);
        /// <summary>
        /// Gets the booking end date.
        /// </summary>
        /// <returns>The booking end date.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        DateTime GetBookingEndDate(BookingIdentifier bookingId);
        /// <summary>
        /// Gets the booking contacts.
        /// </summary>
        /// <returns>The booking contacts.</returns>
        /// <param name="bookingId">Booking identifier.</param>
        List<string> GetBookingContacts(BookingIdentifier bookingId);
    }
}

