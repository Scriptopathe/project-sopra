using System;

namespace SopraProject
{
    /// <summary>
    /// Represents a Reservation identifier.
    /// </summary>
    public class ReservationIdentifier : Identifier<string> 
    {
        public ReservationIdentifier(string id) : base(id) { }
    }
}

