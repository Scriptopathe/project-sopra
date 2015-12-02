using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// Gets the site's identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public SiteIdentifier Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Gets the site name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get 
            { 
                if (_name == String.Empty)
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
        public string Address
        {
            get
            {
                if (_address == String.Empty)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.Site"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public Site(SiteIdentifier id)
        {
            _identifier = id;
        }

        /// <summary>
        /// Gets all the sites in the database.
        /// </summary>
        /// <returns>The all sites.</returns>
        public static List<Site> GetAllSites()
        {
            return ObjectApiProvider.Instance.SitesApi.GetSites().ConvertAll(id => new Site(id));
        }
    }
}

