using System;
using System.Xml.Serialization;

namespace SopraProject.ObjectApi
{
    public class User
    {
        private UserIdentifier _identifier;
        private Site _site;
        private object _lock = new object();
        private bool? _isAdmin;
        #region Properties
        /// <summary>
        /// Gets this site's identifier.
        /// </summary>
        /// <value>The location.</value>
        [XmlIgnore]
        public UserIdentifier Identifier 
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Gets or sets the user's location.
        /// </summary>
        /// <value>The location.</value>
        [XmlIgnore]
        public Site Location
        {
            get 
            {
                lock(_lock)
                {
                    if (_site == null)
                    {
                        _site = Site.Get(ObjectApiProvider.Instance.UserProfileApi.GetLocation(_identifier));
                    }
                }
                return _site;
            }
            set
            {
                ObjectApiProvider.Instance.UserProfileApi.SetLocation(_identifier, value.Identifier);
                _site = value;
            }
        }

        /// <summary>
        /// Gets this user's username.
        /// </summary>
        /// <value>The username.</value>
        [XmlIgnore]
        public string Username
        {
            get
            {
                return _identifier.Value;
            }
        }
        /// <summary>
        /// Gets a value indicating if this user is an administrator.
        /// </summary>
        /// <value>The username.</value>
        [XmlIgnore]
        public bool IsAdmin
        {
            get
            {
                lock(_lock)
                {
                    if (!_isAdmin.HasValue)
                    {
                        _isAdmin = ObjectApiProvider.Instance.UserProfileApi.IsAdmin(Identifier);
                    }
                }
                return _isAdmin.Value;
            }
        }
        #endregion

        #region XML
        [XmlAttribute("id")]
        public string XMLIdentifier
        {
            get { return Identifier.Value; }
            set { _identifier = new UserIdentifier(value); }
        }

        [XmlElement("Username")]
        public string XMLUsername
        {
            get { return Username; }
            set { }
        }

        [XmlElement("Location")]
        public Site XMLLocation
        {
            get { return Location; }
            set { }
        }

        [XmlElement("IsAdmin")]
        public bool XMLIsAdmin
        {
            get { return IsAdmin; }
            set { }
        }

        public User() { }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.ObjectApi.User"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        private User(UserIdentifier id)
        {
            _identifier = id;
        }

        /// <summary>
        /// Authenticate the user with specified username and password.
        /// Returns the corresponding user if it exists, null otherwise.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public static User Authenticate(UserIdentifier username, string password)
        {
            bool userExists = ObjectApiProvider.Instance.AuthApi.Authenticate(username, password);
            if (userExists)
            {
                return new User(username);
            }
            return null;
        }
    }
}

