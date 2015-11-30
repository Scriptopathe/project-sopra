using System;
using System.Collections.Generic;

namespace SopraProject.ObjectApi
{
    public class Reservation
    {
        ReservationIdentifier _identifier;
        DateTime? _startDate;
        DateTime? _endDate;
        string _name;
        List<string> _contacts; 

        /// <summary>
        /// Gets the reservation's contact e-mails.
        /// </summary>
        /// <value>The contacts.</value>
        public IReadOnlyList<string> Contacts
        {
            get 
            {
                if (_contacts == null)
                {
                    _contacts = ObjectApiProvider.Instance.ReservationsApi.GetReservationContacts(_identifier);
                }
                return _contacts;
            }
        }

        /// <summary>
        /// Gets the reservation's start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate
        {
            get 
            {
                if (!_startDate.HasValue)
                {
                    _startDate = ObjectApiProvider.Instance.ReservationsApi.GetReservationStartDate(_identifier);
                }
                return _startDate.Value;
            }
        }

        /// <summary>
        /// Gets the reservation's end date.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime EndDate
        {
            get 
            {
                if (!_endDate.HasValue)
                {
                    _endDate = ObjectApiProvider.Instance.ReservationsApi.GetReservationEndDate(_identifier);
                }
                return _endDate.Value;
            }
        }

        /// <summary>
        /// Gets the reservation's subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject
        {
            get
            {
                if (_name == String.Empty)
                {
                    _name = ObjectApiProvider.Instance.ReservationsApi.GetReservationSubject(_identifier);
                }
                return _name;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.Reservation"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public Reservation(ReservationIdentifier id)
        {
            _identifier = id;
        }
    }
}

