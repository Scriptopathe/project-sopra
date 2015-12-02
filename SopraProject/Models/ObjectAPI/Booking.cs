using System;
using System.Collections.Generic;

namespace SopraProject.ObjectApi
{
    public class Booking
    {
        BookingIdentifier _identifier;
        DateTime? _startDate;
        DateTime? _endDate;
        string _name;
        List<string> _contacts;

        /// <summary>
        /// Gets this booking's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public BookingIdentifier Identifier
        {
            get { return _identifier; }
        }
        /// <summary>
        /// Gets the booking's contact e-mails.
        /// </summary>
        /// <value>The contacts.</value>
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
        public string Subject
        {
            get
            {
                if (_name == String.Empty)
                {
                    _name = ObjectApiProvider.Instance.BookingsApi.GetBookingSubject(_identifier);
                }
                return _name;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.Booking"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public Booking(BookingIdentifier id)
        {
            _identifier = id;
        }
    }
}

