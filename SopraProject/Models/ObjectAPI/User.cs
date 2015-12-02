using System;

namespace SopraProject.ObjectApi
{
    public class User
    {
        private UserIdentifier _identifier;
        private Site _site;

        /// <summary>
        /// Gets or sets the user's location.
        /// </summary>
        /// <value>The location.</value>
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
            }
        }

        /// <summary>
        /// Gets this user's username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get
            {
                return _identifier.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.ObjectApi.User"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public User(UserIdentifier id)
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

