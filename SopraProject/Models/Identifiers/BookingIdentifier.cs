using System;

namespace SopraProject.Models.Identifiers
{
    /// <summary>
    /// Represents a Booking identifier.
    /// </summary>
    public class BookingIdentifier : Identifier<string> 
    {
        public BookingIdentifier(string id) : base(id) { }
    }
}

