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

        #region Properties
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
        #endregion

        #region XML
        [XmlAttribute("id")]
        public string XMLIdentifier
        {
            get { return Identifier.Value; }
            set { _identifier = new BookingIdentifier(value); }
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

