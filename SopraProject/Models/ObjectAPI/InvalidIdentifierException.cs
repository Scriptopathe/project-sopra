using System;

namespace SopraProject.ObjectApi
{
    /// <summary>
    /// Exception raised when an object with an invalid identifier is detected.
    /// </summary>
    public class InvalidIdentifierException : Exception
    {
        private string _identifier;
        private Type _type;
        public override string Message
        {
            get
            {
                return "Object of type " + _type.FullName + " created with an identifier which does not exist in the database : '" + _identifier + "'.";
            }
        }

        public InvalidIdentifierException(Type objectType, string identifierValue) : base() 
        { 
            _identifier = identifierValue;
            _type = objectType;
        }
    }
}

