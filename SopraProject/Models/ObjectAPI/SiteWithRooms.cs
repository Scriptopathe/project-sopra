using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SopraProject.ObjectApi;

namespace SopraProject.ObjectApi
{
    public class SiteWithRooms
    {
        [XmlAttribute("siteId")]
        public string SiteId { get; set; }

        [XmlArray("rooms")]
        public List<Room> Rooms { get; set; }

        public SiteWithRooms()
        {
            Rooms = new List<Room>();
        }

        public SiteWithRooms(string siteId)
        {
            SiteId = siteId;
            Rooms = new List<Room>(Site.Get(new SiteIdentifier(siteId)).Rooms);
        }

    }
}

