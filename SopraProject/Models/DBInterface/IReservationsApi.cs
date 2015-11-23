using System;
using System.Collections.Generic;
namespace SopraProject
{
    public interface IReservationsApi
    {
        /// <summary>
        /// Gets a list of all reservationId
        /// </summary>
        /// <returns>The reservations.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        List<string> GetReservations(DateTime startDate, DateTime endDate);
        string GetReservationSubject(string reservationId);
        string GetReservationRoom(string reservationId);
        DateTime GetReservationStartDate(string reversationId);
        DateTime GetReservationEndDate(string reservationId);
        List<string> GetReservationContacts(string reservationId);
    }
}

