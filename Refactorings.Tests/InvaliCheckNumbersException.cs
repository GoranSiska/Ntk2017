using System;
using System.Runtime.Serialization;

namespace Refactorings.Tests
{
    [Serializable]
    internal class InvaliCheckNumbersException : Exception
    {
        public InvaliCheckNumbersException()
        {
        }

        public InvaliCheckNumbersException(string message) : base(message)
        {
        }

        public InvaliCheckNumbersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvaliCheckNumbersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}