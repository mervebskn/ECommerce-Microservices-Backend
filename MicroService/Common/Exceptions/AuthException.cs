using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class AuthException : Exception
    {
        public int StatusCode { get; private set; }

        public AuthException() : base() { }

        public AuthException(string message) : base(message) { }

        public AuthException(string message, Exception innerException)
            : base(message, innerException) { }

        public AuthException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public AuthException(string message, int statusCode, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }

}
