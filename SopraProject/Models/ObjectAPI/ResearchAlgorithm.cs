using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SopraProject.Tools.Extensions.Date;
using System.Xml.Serialization;
using System.Web.Security;
using SopraProject.Models.Identifiers;

namespace SopraProject.Models.ObjectApi
{


    public class ResearchAlgorithm
    {
        #region Classes
        /// <summary>
        /// Represents a booking candidate.
        /// </summary>
        public class BookingCandidate
        {
            #region XML
            [XmlElement("day")]
            public string XMLDay { get { return StartDate.DayOfWeek.ToString() + " " + StartDate.ToShortDateString(); } set { } }
            [XmlElement("startTime")]
            public string XMLStartDate { get { return StartDate.ToShortTimeString(); } set { } }
            [XmlElement("endTime")]
            public string XMLEndDate { get { return EndDate.ToShortTimeString(); } set { } }
            #endregion

            #region Properties
            [XmlIgnore]
            public DateTime StartDate { get; set; }
            [XmlIgnore]
            public DateTime EndDate { get; set; }
            #endregion

            public BookingCandidate() { }
            public BookingCandidate(DateTime startDate, DateTime endDate)
            {
                StartDate = startDate;
                EndDate = endDate;
            }
        }
        /// <summary>
        /// Represents a room search result/
        /// </summary>
        public class RoomSearchResult
        {
            /// <summary>
            /// Room data.
            /// </summary>
            [XmlElement("Room")]
            public Room Room { get; set; }

            /// <summary>
            /// List of the booking candidates the search algorithm found.
            /// </summary>
            [XmlArray("Bookings")]
            public List<BookingCandidate> BookingCandidates { get; set; }

            public RoomSearchResult() { }
            public RoomSearchResult(Room room)
            {
                Room = room;
                BookingCandidates = new List<BookingCandidate>();
            }
        }
        #endregion
        /// <summary>
        /// Creates a new instance of the search algorithm.
        /// </summary>
        public ResearchAlgorithm()
        {
        }

        
        public List<RoomSearchResult> Search(int siteId, int minCapacity, string[] particularities, int meetingDurationMinutes, DateTime startDate, DateTime endDate)
        {
            var conf = Configuration.Provider.Instance.Search;
            List<Room> rooms = siteId == -1 ? Room.GetAllRooms() : new List<Room>(Site.Get(new SiteIdentifier(siteId.ToString())).Rooms);
            // Preprocess the start date : minutes must be dividible by MIN_MEETING_DURATION.
            startDate = startDate.AddMinutes(-(startDate.Minute % conf.MinMeetingDuration)).AddSeconds(-startDate.Second);

            // Filter room capacity.
            if (minCapacity != -1)
                rooms = rooms.FindAll(room => room.Capacity >= minCapacity);

            // Filters particularities
            if (particularities != null)
            {
                List<Room> filteredRooms = new List<Room>();
                foreach (Room room in rooms)
                {
                    var parts = room.Particularities.ToList().ConvertAll(p => p.Identifier.Value);
                    var intersection = parts.Intersect(particularities);
                    if (intersection.Count() == particularities.Count())
                    {
                        filteredRooms.Add(room);
                    }
                }
                rooms = filteredRooms;
            }

            List<RoomSearchResult> results = new List<RoomSearchResult>();
            foreach(Room room in rooms)
            {
                RoomSearchResult result = new RoomSearchResult(room);
                List<Booking> allBookings = ObjectApiProvider.Instance.BookingsApi.GetBookings(room.Identifier, startDate, endDate).ConvertAll(id => Booking.Get(id));

                // Naive implementation : look at each 15min period to see if it is not empty.
                DateTime lastDate = endDate.AddMinutes(-meetingDurationMinutes);
                DateTime? meetingStart = null;
                for(DateTime currentDate = startDate; currentDate <= lastDate; currentDate = currentDate.AddMinutes(conf.MinMeetingDuration))
                {
                    DateTime meetingEndDate = currentDate.AddMinutes(meetingDurationMinutes);
                    var bookings = allBookings.Where(booking => booking.EndDate > currentDate && booking.StartDate <= meetingEndDate);
                    bool record = false;
                    bool dayJump = false;
                    if (bookings.Count() == 0)
                    {
                        // Here the slot from currentDate to meetingEndDate is available.
                        if(!meetingStart.HasValue)
                        {
                            // if no meeting start date is set, then sets it to the first available
                            meetingStart = currentDate;
                        }
                    }
                    else
                    {
                        // Add the contiguous reservable block if it exists. 
                        record = true;
                    }

                    // If we are in the last iteration, record
                    if (currentDate >= lastDate)
                        record = true;

                    // If we get to the end of the day, start searching from the start of the next day
                    if(currentDate.Hour >= conf.DayEnd)
                    {
                        dayJump = true;
                        record = true;
                    }

                    // Records the last block
                    if (record)
                    {
                        if (meetingStart.HasValue && (currentDate - meetingStart.Value).TotalMinutes >= meetingDurationMinutes)
                        {
                            result.BookingCandidates.Add(new BookingCandidate(meetingStart.Value, currentDate));
                        }
                        meetingStart = null;
                    }

                    // Jumps to the next day
                    if(dayJump)
                        currentDate = currentDate.AddHours(24 - conf.DayEnd + conf.DayStart).AddMinutes(-conf.MinMeetingDuration);

                }

                // Add last result

                results.Add(result);
            }

            return results;
        }
        

        public List<Room> Search(int siteId = -1, int personCount=-1, string[] particularities=null, DateTime? startDate = null, DateTime? endDate = null)
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

