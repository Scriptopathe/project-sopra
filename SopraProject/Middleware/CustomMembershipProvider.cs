using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
namespace SopraProject
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override MembershipUser CreateUser(string username, string password, 
            string email, string passwordQuestion, string passwordAnswer, 
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = 
                new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser user = GetUser(username, true);

            if (user == null)
            {
                UserObj userObj = new UserObj();
                userObj.UserName = username;
                userObj.Password = GetMD5Hash(password);
                userObj.UserEmailAddress = email;

                User userRep = new User();
                userRep.RegisterUser(userObj);

                status = MembershipCreateStatus.Success;

                return GetUser(username, true);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            User userRep = new User();
            UserObj user = userRep.GetAllUsers().SingleOrDefault
                (u => u.UserName == username);
            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("CustomMembershipProvider", 
                    username, user.UserID, user.UserEmailAddress,
                    string.Empty, string.Empty,
                    true, false, DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }

        public override bool ValidateUser(string username, string password)
        {
            string sha1Pswd = GetMD5Hash(password);
            User user = new User();
            UserObj userObj = user.GetUserObjByUserName(username, sha1Pswd);
            if (userObj != null)
                return true;
            return false;
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override bool RequiresUniqueEmail
        {
            // In a real application, you will essentially have to return true
            // and implement the GetUserNameByEmail method to identify duplicates
            get { return false; }
        }

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}

