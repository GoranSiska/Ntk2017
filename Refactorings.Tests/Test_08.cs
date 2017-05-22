using NUnit.Framework;
using System;
using System.Linq;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_08 //Replace Nested Conditional with Guard Clauses
    {
        [Test]
        public void GivenCustomerRepository_Revisited_Before_WhenStoreCustomerWithInvalidBankAccountNumber_ThrowsException()
        {
            var customer = new Customer();
            customer.BankAccountNumber = "12345678901234567890123456789012345";
            var repository = new CustomerRepository_Revisited_Before();

            Assert.Throws<InvalidBankAccountException>(() => repository.StoreCustomer(customer));
        }

        [Test]
        public void GivenCustomerRepository_Revisited_After_WhenValidateInvalidBankAccountNumber_ThrowsException()
        {
            var customer = new Customer();
            customer.BankAccountNumber = "12345678901234567890123456789012345";
            var repository = new CustomerRepository_Revisited_After();

            Assert.Throws<InvalidBankAccountException>(() => repository.StoreCustomer(customer));
        }
    }


    #region Example 1

    /*
        Deep nesting makes StoreCustomer method hard to read 
    */
    public class CustomerRepository_Revisited_Before
    {
        public Customer StoreCustomer(Customer customer)
        {
            if (customer.BankAccountNumber.Length <= 34)
            {
                var countryCode = customer.BankAccountNumber.Substring(0, 2);
                if (ValidateCountryCodeExists(countryCode))
                {
                    var checkNumbers = customer.BankAccountNumber.Substring(2, 2);
                    if (ValidateCheckNumbers(checkNumbers))
                    {
                        var basicAccountNumberLength = customer.BankAccountNumber.Length - 4;
                        var basicAccountNumber = customer.BankAccountNumber.Substring(4, basicAccountNumberLength);

                        if (basicAccountNumber.All(n => char.IsDigit(n))) //we have problems with this code in china!
                        {
                            var storedCustomer = StoreCustomerImplementation(customer);

                            return storedCustomer;
                        }
                        else
                        {
                            throw new InvalidBasicBankAccountNumberException();
                        }
                    }
                    else
                    {
                        throw new InvaliCheckNumbersException();
                    }
                }
                else
                {
                    throw new InvaliCountryCodeException();
                }
            }
            else
            {
                throw new InvalidBankAccountException();
            }
        }

        #region Helpers

        private Customer StoreCustomerImplementation(Customer customer)
        {
            return customer;
        }

        private bool ValidateCheckNumbers(string checkNumbers)
        {
            throw new NotImplementedException();
        }

        private bool ValidateCountryCodeExists(string countryCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /*
        Guard clauses make StoreCustomer method easier to read.
        Easy to see where early returns happen.
    */
    public class CustomerRepository_Revisited_After
    {
        public Customer StoreCustomer(Customer customer)
        {
            if (customer.BankAccountNumber.Length > 34)
            {
                throw new InvalidBankAccountException();
            }

            var countryCode = customer.BankAccountNumber.Substring(0, 2);
            var checkNumbers = customer.BankAccountNumber.Substring(2, 2);
            var basicAccountNumberLength = customer.BankAccountNumber.Length - 4;
            var basicAccountNumber = customer.BankAccountNumber.Substring(4, basicAccountNumberLength);

            if (!ValidateCountryCodeExists(countryCode))
            {
                throw new InvaliCountryCodeException();
            }

            if (!ValidateCheckNumbers(checkNumbers))
            {
                throw new InvaliCheckNumbersException();
            }

            if (!basicAccountNumber.All(n => char.IsDigit(n)))
            {
                throw new InvalidBasicBankAccountNumberException();
            }

            var storedCustomer = StoreCustomerImplementation(customer);

            return storedCustomer;
        }

        #region Helpers

        private Customer StoreCustomerImplementation(Customer customer)
        {
            return customer;
        }

        private bool ValidateCheckNumbers(string checkNumbers)
        {
            throw new NotImplementedException();
        }

        private bool ValidateCountryCodeExists(string countryCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #endregion
}

