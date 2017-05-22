using System;
using System.Runtime.Serialization;

namespace Refactorings.Tests
{
    [Serializable]
    internal class InvalidBasicBankAccountNumberException : Exception
    {
        public InvalidBasicBankAccountNumberException()
        {
        }

        public InvalidBasicBankAccountNumberException(string message) : base(message)
        {
        }

        public InvalidBasicBankAccountNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidBasicBankAccountNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}