using System;
using System.Collections.Generic;
namespace SopraProject
{
    /// <summary>
    /// This interface describes the messages that can be send to get informations
    /// about reservations.
    /// This information is needed to perform room searching with the availability constraint.
    /// 
    /// The final implementation of this will be to look up for the reservations in the 
    /// outlook database.
    /// </summary>
    public interface IReservationsApi
    {
        /// <summary>
        /// Gets a list containing every reservation between the given startDate and endDate.
        /// The reservations which are not totally covered by the given time period (startDate -> endDate)
        /// are also included.
        /// The reservations are represented by their id.
        /// </summary>
        /// <returns>The reservations.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        List<ReservationIdentifier> GetReservations(DateTime startDate, DateTime endDate);
        /// <summary>
        /// Gets the reservation subject.
        /// </summary>
        /// <returns>The reservation subject.</returns>
        /// <param name="reservationId">Reservation identifier.</param>
        string GetReservationSubject(ReservationIdentifier reservationId);
        /// <summary>
        /// Gets the reservation room.
        /// </summary>
        /// <returns>The reservation room.</returns>
        /// <param name="reservationId">Reservation identifier.</param>
        string GetReservationRoom(ReservationIdentifier reservationId);
        /// <summary>
        /// Gets the reservation start date.
        /// </summary>
        /// <returns>The reservation start date.</returns>
        /// <param name="reversationId">Reversation identifier.</param>
        DateTime GetReservationStartDate(ReservationIdentifier reversationId);
        /// <summary>
        /// Gets the reservation end date.
        /// </summary>
        /// <returns>The reservation end date.</returns>
        /// <param name="reservationId">Reservation identifier.</param>
        DateTime GetReservationEndDate(ReservationIdentifier reservationId);
        /// <summary>
        /// Gets the reservation contacts.
        /// </summary>
        /// <returns>The reservation contacts.</returns>
        /// <param name="reservationId">Reservation identifier.</param>
        List<string> GetReservationContacts(ReservationIdentifier reservationId);
    }
}

