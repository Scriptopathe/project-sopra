using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace SopraProject.ObjectApi
{
    public class Room
    {
        #region Variables
        RoomIdentifier _identifier;
        int _capacity = -1;
        string _name;
        List<Particularity> _particularities;
        #endregion

        #region Properties
        /// <summary>
        /// Gets this room's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [XmlIgnore()]
        public RoomIdentifier Identifier 
        {
            get { return _identifier; }
            private set
            {
                _identifier = value;
                if (!ObjectApiProvider.Instance.SitesApi.RoomExists(_identifier))
                    throw new InvalidIdentifierException(this.GetType(), _identifier.Value.ToString());
            }
        }
        /// <summary>
        /// Gets the bookings affected to this room and occurring between the given
        /// start date and end date.
        /// </summary>
        /// <returns>The bookings.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public IReadOnlyList<Booking> GetBookings(DateTime startDate, DateTime endDate)
        {
            return ObjectApiProvider.Instance.BookingsApi.GetBookings(_identifier, startDate, endDate).ConvertAll(s => new Booking(s));
        }

        /// <summary>
        /// Gets the room name.
        /// </summary>
        /// <value>The name.</value>
        [XmlIgnore()]
        public string Name
        {
            get
            { 
                if (_name == null)
                {
                    _name = ObjectApiProvider.Instance.SitesApi.GetRoomName(_identifier);
                }
                return _name;
            }
        }

        /// <summary>
        /// Gets the room capacity (maximum number of people present in the room).
        /// </summary>
        /// <value>The capacity.</value>
        [XmlIgnore()]
        public int Capacity
        {
            get
            {
                if (_capacity == -1)
                {
                    _capacity = ObjectApiProvider.Instance.SitesApi.GetRoomCapacity(_identifier);
                }
                return _capacity;
            }
        }

        /// <summary>
        /// Gets the list of particularties of this room.
        /// </summary>
        /// <value>The particularities.</value>
        [XmlIgnore()]
        public IReadOnlyList<Particularity> Particularities
        {
            get
            {
                if (_particularities == null)
                {
                    _particularities = new List<Particularity>();
                    var roomIds = ObjectApiProvider.Instance.SitesApi.GetRoomParticularities(_identifier);
                    foreach (var roomId in roomIds)
                    {
                        _particularities.Add(new Particularity(roomId));
                    }
                }

                return _particularities;
            }
        }
        #endregion

        #region XML
        [XmlAttribute("id")]
        public string XMLIdentifier
        {
            get { return Identifier.Value; }
            set { _identifier = new RoomIdentifier(value); }
        }

        [XmlElement("Name")]
        public string XMLName
        {
            get { return Name; }
            set { }
        }
        [XmlElement("Capacity")]
        public int XMLCapacity
        {
            get { return Capacity; }
            set { }
        }

        [XmlArray("Particularities")]
        public List<Particularity> XMLParticularities
        {
            get { return (List<Particularity>)Particularities; }
            set { }
        }

        public Room() { }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.ObjectApi.Room"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public Room(RoomIdentifier id)
        {
            Identifier = id;
        }




        #region Static
        /// <summary>
        /// Gets a list containing all rooms of the database.
        /// </summary>
        /// <returns>The all rooms.</returns>
        public static List<Room> GetAllRooms()
        {
            return ObjectApiProvider.Instance.SitesApi.GetRooms().ConvertAll(id => new Room(id));
        }

        /*public List<ParticularityIdentifier> GetParticularities()
        {
            List<ParticularityIdentifier> val = null;
            foreach (var part in Particularities)
            {
                val.Add(part.Identifier);
            }
            //Particularities.
            return val;
        }*/
        #endregion
    }
}

