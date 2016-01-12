using System;

namespace SopraProject.Models.Identifiers
{
    /// <summary>
    /// Represents a Room identifier.
    /// </summary>
    public class RoomIdentifier : Identifier<string> 
    { 
        public RoomIdentifier(string id) : base(id) { }
    }
}