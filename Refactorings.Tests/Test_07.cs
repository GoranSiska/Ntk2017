using NUnit.Framework;
using System;
using System.Linq;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_07 //Replace Methods with Method Objects
    {
        [Test]
        public void GivenCustomerRepository_Revisited_Again_Before_WhenStoreCustomerWithInvalidBankAccountNumber_ThrowsException()
        {
            var customer = new Customer();
            customer.BankAccountNumber = "12345678901234567890123456789012345";
            var repository = new CustomerRepository_Revisited_Again_Before();

            Assert.Throws<InvalidBankAccountException>(() => repository.StoreCustomer(customer));
        }

        [Test]
        public void GivenBankAccountValidator_WhenValidateInvalidBankAccountNumber_ThrowsException()
        {
            var bankAccountNumber = "12345678901234567890123456789012345";
            var validator = new BankAccountValidator(bankAccountNumber);

            Assert.Throws<InvalidBankAccountException>(() => validator.Validate());
        }
    }

    #region Example 1

    /*
        Validation code can not be reused / is part of persistence logic.
    */
    public class CustomerRepository_Revisited_Again_Before
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

    public class BankAccountValidator
    {
        readonly string _bankAccountNumber;
        readonly string _countryCode;
        readonly string _checkNumbers;
        readonly string _basicAccountNumber;
        public BankAccountValidator(string bankAccountNumber)
        {
            _bankAccountNumber = bankAccountNumber;

            var basicAccountNumberLength = bankAccountNumber.Length - 4;

            _countryCode = bankAccountNumber.Substring(0, 2);
            _checkNumbers = bankAccountNumber.Substring(2, 2);
            _basicAccountNumber = bankAccountNumber.Substring(4, basicAccountNumberLength);
        }

        public void Validate()
        {
            ValidateBankAccountLength();
            ValidateCountryCode();
            ValidateCheckNumbers();
            ValidateBasicAccountNumber();
        }

        public void ValidateBankAccountLength()
        {
            if (_bankAccountNumber.Length > 34)
            {
                throw new InvalidBankAccountException();
            }
        }

        public void ValidateCountryCode()
        {
            if (!ValidateCountryCodeExists(_countryCode))
            {
                throw new InvaliCountryCodeException();
            }
        }
        public void ValidateCheckNumbers()
        {
            if (!ValidateCheckNumbers(_checkNumbers))
            {
                throw new InvaliCheckNumbersException();
            }
        }

        public void ValidateBasicAccountNumber()
        {
            if (!_basicAccountNumber.All(n => char.IsDigit(n)))
            {
                throw new InvalidBasicBankAccountNumberException();
            }
        }

        #region Helpers

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
        Validation code is extracted to separate class.
        Can be reused / tested.
        Does not depend on persistence.
    */

    public class CustomerRepository_Revisited_Again_After
    {
        public Customer StoreCustomer(Customer customer)
        {
            var bankAccountValidator = new BankAccountValidator(customer.BankAccountNumber);
            bankAccountValidator.Validate();

            var storedCustomer = StoreCustomerImplementation(customer);

            return storedCustomer;
        }

        #region Helpers

        private Customer StoreCustomerImplementation(Customer customer)
        {
            return customer;
        }

        #endregion
    }

    #endregion
}

