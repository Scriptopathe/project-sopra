using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace SopraProject.ObjectApi
{
    public class Booking
    {
        BookingIdentifier _identifier;
        DateTime? _startDate;
        DateTime? _endDate;
        string _name;
        List<string> _contacts;
        Room _room;
        int? _participantsCount;

        #region Properties
        /// <summary>
        /// Gets the duration of the booking in hours.
        /// </summary>
        public float Duration
        {
            get { return (this.EndDate - this.StartDate).Minutes / 60.0f; }
        }

        /// <summary>
        /// Gets this booking's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [XmlIgnore()]
        public BookingIdentifier Identifier
        {
            get { return _identifier; }
            private set
            {
                _identifier = value;
                if (!ObjectApiProvider.Instance.BookingsApi.BookingExists(_identifier))
                    throw new InvalidIdentifierException(this.GetType(), _identifier.Value.ToString());
            }
        }

        /// <summary>
        /// Gets the room.
        /// </summary>
        /// <value>The room.</value>
        [XmlIgnore()]
        public Room Room
        {
            get 
            { 
                if (_room == null)
                {
                    _room = Room.Get(ObjectApiProvider.Instance.BookingsApi.GetBookingRoom(_identifier));
                }
                return _room; 
            }
        }

        /// <summary>
        /// Gets the booking's contact e-mails.
        /// </summary>
        /// <value>The contacts.</value>
        [XmlIgnore()]
        public IReadOnlyList<string> Contacts
        {
            get 
            {
                if (_contacts == null)
                {
                    _contacts = ObjectApiProvider.Instance.BookingsApi.GetBookingContacts(_identifier);
                }
                return _contacts;
            }
        }

        /// <summary>
        /// Gets the booking's start date.
        /// </summary>
        /// <value>The start date.</value>
        [XmlIgnore()]
        public DateTime StartDate
        {
            get 
            {
                if (!_startDate.HasValue)
                {
                    _startDate = ObjectApiProvider.Instance.BookingsApi.GetBookingStartDate(_identifier);
                }
                return _startDate.Value;
            }
        }

        /// <summary>
        /// Gets the booking's end date.
        /// </summary>
        /// <value>The end date.</value>
        [XmlIgnore()]
        public DateTime EndDate
        {
            get 
            {
                if (!_endDate.HasValue)
                {
                    _endDate = ObjectApiProvider.Instance.BookingsApi.GetBookingEndDate(_identifier);
                }
                return _endDate.Value;
            }
        }

        /// <summary>
        /// Gets the booking's subject.
        /// </summary>
        /// <value>The subject.</value>
        [XmlIgnore()]
        public string Subject
        {
            get
            {
                if (_name == null)
                {
                    _name = ObjectApiProvider.Instance.BookingsApi.GetBookingSubject(_identifier);
                }
                return _name;
            }
        }

        /// <summary>
        /// Gets the number of participants of the booking.
        /// </summary>
        [XmlIgnore()]
        public int ParticipantsCount
        {
            get
            {
                if(!_participantsCount.HasValue)
                {
                    _participantsCount = ObjectApiProvider.Instance.BookingsApi.GetBookingParticipantsCount(_identifier);
                }
                return _participantsCount.Value;
            }
        }
        #endregion

        #region XML
        [XmlAttribute("id")]
        public string XMLIdentifier
        {
            get { return Identifier.Value; }
            set { _identifier = new BookingIdentifier(value); }
        }

        [XmlElement("Room")]
        public Room XMLRoom
        {
            get { return Room;}
            set { }
        }

        [XmlElement("Contacts")]
        public List<string> XMLContacts 
        {
            get { return (List<string>)Contacts; }
            set { }
        }

        [XmlElement("StartDate")]
        public DateTime XMLStartDate
        {
            get { return StartDate; }
            set { }
        }
        [XmlElement("EndDate")]
        public DateTime XMLEndDate
        {
            get { return EndDate; }
            set { }
        }
        [XmlElement("Subject")]
        public string XMLSubject
        {
            get { return Subject; }
            set { }
        }
        public Booking() { }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.Booking"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public Booking(BookingIdentifier id)
        {
            Identifier = id;
        }
    }
}

