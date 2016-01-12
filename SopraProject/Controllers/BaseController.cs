using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SopraProject.Models.ObjectApi;
using System.Xml.Serialization;

namespace SopraProject.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Returns the current authenticated user.
        /// </summary>
        protected User GetUser()
        {
            return (User)Session["User"];
        }


        #region Parameter Checking
        /// <summary>
        /// Use this function to return an error to the API.
        /// </summary>
        protected ActionResult Error(string message)
        {
            Response.Write(message);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Throws an exception if the given value is not positive.
        /// </summary>
        protected int CheckIsPositive(int value)
        {
            if (value < 0)
                throw new Exception("Value must be positive.");
            return value;
        }

        /// <summary>
        /// Throws an exception if the given value is in the given range.
        /// </summary>
        protected int CheckRange(int value, int min, int max)
        {
            if (value < min || value > max)
                throw new Exception("Value must be in range [" + min + ", " + max + "].");
            return value;
        }

        public class ParameterCheckException : Exception { public ParameterCheckException(string msg) : base(msg) { } }

        /// <summary>
        /// Executes the given function. If the function throws an exception, it is wrapped into a ParameterCheckException 
        /// with a given message and parameter name.
        /// This exception contains a serialized XML object containg details about the error to be sent to the client.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function to be executed.</param>
        /// <param name="parameterName">The name of the parameter which is currently checked.</param>
        /// <param name="errorMessage">The error message to display if an error occurs. 
        /// if null or not specified, the message is taken from the underlaying exception.</param>
        /// <returns></returns>
        protected T Checked<T>(Func<T> func, string parameterName, string errorMessage = null)
        {
            T obj;
            try
            {
                obj = func();
            }
            catch (Exception e)
            {
                if (errorMessage == null)
                    errorMessage = e.Message;
                throw new ParameterCheckException(new InputError() { ParameterName = parameterName, Message = errorMessage }.ToString());
            }
            return obj;
        }
        #endregion
    }

    /// <summary>
    /// Represents an input error.
    /// </summary>
    public class InputError
    {
        /// <summary>
        /// Gets the name of the input parameter which was incorrect..
        /// </summary>
        [XmlAttribute("name")]
        public string ParameterName { get; set; }
        /// <summary>
        /// Gets the message to display to the user.
        /// </summary>
        [XmlAttribute("message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return Tools.Serializer.Serialize(this);
        }
    }
}
