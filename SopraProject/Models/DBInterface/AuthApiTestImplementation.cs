using System;
using System.Linq;
namespace SopraProject
{
    /// <summary>
    /// Auth API test implementation.
    /// </summary>
    public class AuthApiTestImplementation : IAuthApi
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SopraProject.AuthApiImplementationTest"/> class.
        /// </summary>
		public AuthApiTestImplementation()
		{
            
		}

        /// <summary>
        /// Authenticate the User with specified username and password.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public bool Authenticate(UserIdentifier username, string password)
        {
            bool isOk = false;
            using (var ctx = new SopraProject.Database.AuthenticationContext())
            {
                var query = from user in ctx.Users
                            where user.Username.Equals(username.Value) && user.Password.Equals(password)
                            select user.Username;
                isOk = query.Count() != 0;
            }
            return isOk;
        }
	}
}

