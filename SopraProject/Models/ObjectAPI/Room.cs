using System;
using System.Collections.Generic;

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
        /// Gets the room name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            { 
                if (_name == String.Empty)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.ObjectApi.Room"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public Room(RoomIdentifier id)
        {
            _identifier = id;
        }
    }
}

