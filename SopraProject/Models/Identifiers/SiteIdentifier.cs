using System;

namespace SopraProject.Models.Identifiers
{
    /// <summary>
    /// Represents a Site identifier.
    /// </summary>
    public class SiteIdentifier : Identifier<string> 
    { 
        public SiteIdentifier(string id) : base(id) { }
    }
}

