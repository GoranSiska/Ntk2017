using System;
using System.Runtime.Serialization;

namespace Refactorings.TestClasses.Business
{
    [Serializable]
    internal class InvalidBankAccountException : Exception
    {
        public InvalidBankAccountException()
        {
        }

        public InvalidBankAccountException(string message) : base(message)
        {
        }

        public InvalidBankAccountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidBankAccountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}