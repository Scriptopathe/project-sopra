using System;
using System.Text;
using System.Security.Cryptography;
namespace SopraProject.Tools
{
    public static class Security
    {
        /// <summary>
        /// Creates an Base64 digest of the SHA1 hash of this password.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>The password SHA1 hash (base64 format)</returns>
        public static string Hash(string password)
        {
            byte[] pwbytes = Encoding.UTF8.GetBytes(password);
            string hashString;
            using (SHA1 sha1 = SHA1.Create())
            {
                hashString = Convert.ToBase64String(sha1.ComputeHash(pwbytes));
            }
            return hashString;
        }
    }
}

