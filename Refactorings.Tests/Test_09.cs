using NUnit.Framework;
using System;
using System.Linq;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_09 //Refactor Routines to Appropriate Level of Abstraction
    {
        [Test]
        public void GivenCustomerRepository_Before_WhenStoreCustomerWithInvalidBankAccountNumber_ThrowsException()
        {
            var customer = new Customer();
            customer.BankAccountNumber = "12345678901234567890123456789012345";
            var repository = new CustomerRepository_Before();

            Assert.Throws<InvalidBankAccountException>(() => repository.StoreCustomer(customer));
        }

        [Test]
        public void GivenCustomerRepository_After_WhenValidateInvalidBankAccountNumber_ThrowsException()
        {
            var bankAccountNumber = "12345678901234567890123456789012345";
            var repository = new CustomerRepository_After();

            Assert.Throws<InvalidBankAccountException>(() => repository.ValidateBankAccountNumber(bankAccountNumber));
        }
    }


    #region Example 1

    /*
        Mixed validation and persistance code.
        Parsing strings alongside abstract operations.
    */
    public class CustomerRepository_Before
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

        public virtual Customer StoreCustomerImplementation(Customer customer)
        {
            return customer;
        }

        public virtual bool ValidateCheckNumbers(string checkNumbers)
        {
            throw new NotImplementedException();
        }

        public virtual bool ValidateCountryCodeExists(string countryCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /*
        Equal level of abstraction.
        Parsing strings extracted to testable method.
    */
    public class CustomerRepository_After
    {
        public Customer StoreCustomer(Customer customer)
        {
            ValidateBankAccountNumber(customer.BankAccountNumber);
            var storedCustomer = StoreCustomerImplementation(customer);

            return storedCustomer;
        }

        public void ValidateBankAccountNumber(string bankAccountNumber)
        {
            if (bankAccountNumber.Length <= 34)
            {
                var countryCode = bankAccountNumber.Substring(0, 2);
                if (ValidateCountryCodeExists(countryCode))
                {
                    var checkNumbers = bankAccountNumber.Substring(2, 2);
                    if (ValidateCheckNumbers(checkNumbers))
                    {
                        var basicAccountNumberLength = bankAccountNumber.Length - 4;
                        var basicAccountNumber = bankAccountNumber.Substring(4, basicAccountNumberLength);

                        if (!basicAccountNumber.All(n => char.IsDigit(n))) //we have problems with this code in china!
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

    #endregion
}

