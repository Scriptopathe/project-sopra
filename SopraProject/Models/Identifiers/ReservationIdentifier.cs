using System;

namespace SopraProject
{
    /// <summary>
    /// Represents a Room identifier.
    /// </summary>
    public class RoomIdentifier : Identifier<string> 
    { 
        public RoomIdentifier(string id) : base(id) { }
    }
}

