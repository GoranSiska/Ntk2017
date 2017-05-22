using System;

namespace Refactorings.TestClasses.Business
{
    public class CustomerDataNotAvailableException : Exception
    {
        public CustomerDataNotAvailableException(int customerId, Exception innerException) : base(GetMessage(customerId), innerException)
        { }

        private static string GetMessage(int customerId)
        {
            return string.Format("Failed to retreve data for customer with id:{0}", customerId);
        }
    }
}

