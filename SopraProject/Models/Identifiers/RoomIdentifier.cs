using System;

namespace SopraProject
{
    /// <summary>
    /// Represents a Site identifier.
    /// </summary>
    public class RoomIdentifier : Identifier<string> 
    { 
        public RoomIdentifier(string id) : base(id) { }
    }
}

