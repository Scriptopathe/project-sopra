using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SopraProject.Tools.Extensions.Date;
using System.Xml.Serialization;
using System.Web.Security;
using SopraProject.ObjectApi;

namespace SopraProject.ObjectApi
{
    public class ResearchAlgorithm
    {
        public ResearchAlgorithm()
        {
        }

        public List<Room> research(int siteId = -1, int personCount=-1, string[] particularities=null, DateTime? startDate = null, DateTime? endDate = null)
        {
            List<Room> rooms = null;
            if (siteId != -1) {
                rooms = new List<Room> (Site.Get (new SiteIdentifier (siteId.ToString ())).Rooms);

            } else {
                rooms = Room.GetAllRooms();
            }
            if (personCount != -1)
            {
                rooms = rooms.FindAll (room => room.Capacity >= personCount);
            }
            if (particularities != null)
            {
                List<Room> filteredRooms = new List<Room> ();
                foreach(Room room in rooms)
                {
                    var parts = room.Particularities.ToList().ConvertAll(p => p.Identifier.Value);
                    var intersection = parts.Intersect (particularities);
                    if (intersection.Count () == particularities.Count ()) {
                        filteredRooms.Add (room);
                    }
                }
                rooms = filteredRooms;
            }

            if (startDate != null && endDate != null)
            {
                List<Room> filteredRooms = new List<Room> ();
                foreach(Room room in rooms)
                {
                    var bookings = ObjectApiProvider.Instance.BookingsApi.GetBookings(room.Identifier, startDate.Value, endDate.Value);
                    if (bookings.Count == 0)
                    {
                         filteredRooms.Add(room);
                    }
                }
                rooms = filteredRooms;
            }

            return rooms;
        }
    }
}

