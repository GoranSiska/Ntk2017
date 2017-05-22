using System;
using System.Runtime.Serialization;

namespace Refactorings.TestClasses.Business
{
    [Serializable]
    internal class InvaliCountryCodeException : Exception
    {
        public InvaliCountryCodeException()
        {
        }

        public InvaliCountryCodeException(string message) : base(message)
        {
        }

        public InvaliCountryCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvaliCountryCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}