using System;
using System.Xml.Serialization;

namespace SopraProject.ObjectApi
{
    public class User
    {
        private UserIdentifier _identifier;
        private Site _site;

        #region Properties
        /// <summary>
        /// Gets this site's identifier.
        /// </summary>
        /// <value>The location.</value>
        [XmlIgnore()]
        public UserIdentifier Identifier 
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Gets or sets the user's location.
        /// </summary>
        /// <value>The location.</value>
        [XmlIgnore()]
        public Site Location
        {
            get 
            {
                if (_site == null)
                {
                    _site = new Site(ObjectApiProvider.Instance.UserProfileApi.GetLocation(_identifier));
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
        [XmlIgnore()]
        public string Username
        {
            get
            {
                return _identifier.Value;
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

