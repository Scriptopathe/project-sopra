using System;

namespace SopraProject
{
    /// <summary>
    /// Represents a Site identifier.
    /// </summary>
    public class SiteIdentifier : Identifier<string> 
    { 
        public SiteIdentifier(string id) : base(id) { }
    }
}

