using System;

namespace SopraProject
{
    /// <summary>
    /// Represents a Booking identifier.
    /// </summary>
    public class BookingIdentifier : Identifier<string> 
    {
        public BookingIdentifier(string id) : base(id) { }
    }
}

