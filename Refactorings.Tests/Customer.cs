namespace Refactorings.Tests
{
    public class Customer : Person
    {
        public int Id { get; set; }
        public string Name { get; internal set; }
        public int Age { get; internal set; }
        public string BankAccountNumber { get; internal set; }
    }
}

