using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace SopraProject.ObjectApi
{
    /// <summary>
    /// Sites.
    /// </summary>
    public class Site
    {
        private SiteIdentifier _identifier;
        private string _name;
        private string _address;
        private List<Room> _rooms;

        #region Properties
        /// <summary>
        /// Gets the site's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [XmlIgnore()]
        public SiteIdentifier Identifier
        {
            get { return _identifier; }            
            private set
            {
                _identifier = value;
                if (!ObjectApiProvider.Instance.SitesApi.SiteExists(_identifier))
                    throw new InvalidIdentifierException(this.GetType(), _identifier.Value.ToString());
            }
        }

        /// <summary>
        /// Gets the site name.
        /// </summary>
        /// <value>The name.</value>
        [XmlIgnore()]
        public string Name
        {
            get 
            { 
                if (_name == null)
                {
                    _name = ObjectApiProvider.Instance.SitesApi.GetSiteName(_identifier);
                }
                return _name;
            }
        }

        /// <summary>
        /// Gets the site address.
        /// </summary>
        /// <value>The address.</value>
        [XmlIgnore()]
        public string Address
        {
            get
            {
                if (_address == null)
                {
                    _address = ObjectApiProvider.Instance.SitesApi.GetSiteAddress(_identifier);
                }
                return _address;
            }
        }

        /// <summary>
        /// Gets the list of rooms in this site.
        /// </summary>
        /// <value>The rooms.</value>
        [XmlIgnore()]
        public IReadOnlyList<Room> Rooms
        {
            get
            {
                if (_rooms == null)
                {
                    _rooms = new List<Room>();
                    var roomIds = ObjectApiProvider.Instance.SitesApi.GetRooms(_identifier);
                    foreach (var roomId in roomIds)
                    {
                        _rooms.Add(new Room(roomId));
                    }
                }

                return _rooms;
            }
        }

        #region XML
        [XmlAttribute("id")]
        public string XMLIdentifier
        {
            get { return Identifier.Value; }
            set { _identifier = new SiteIdentifier(value); }
        }

        [XmlElement("Name")]
        public string XMLName
        {
            get { return Name; }
            set { }
        }
        [XmlElement("Address")]
        public string XMLAddress
        {
            get { return Address; }
            set { }
        }

        /*[XmlArray("Rooms")]
        public List<Room> XMLRooms
        {
            get { return (List<Room>)Rooms; }
            set { }
        }*/

        public Site() { }
        #endregion

        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.Site"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public Site(SiteIdentifier id)
        {
            Identifier = id;
        }


        /// <summary>
        /// Gets all the sites in the database.
        /// </summary>
        /// <returns>The all sites.</returns>
        public static List<Site> GetAllSites()
        {
            return ObjectApiProvider.Instance.SitesApi.GetSites().ConvertAll(id => new Site(id));
        }

        public static int GetSitesCount()
        {
            return ObjectApiProvider.Instance.SitesApi.SitesCount();
        }
    }
}

