using NUnit.Framework;
using System;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_03 // Replace Conditionals with Polymorphism
    {
        [Test]
        public void GivenPartnerRepository_Before_WhenGenerateReferenceNumberForIndividual_CorrectIdIsReturned()
        {
            var partner = new Individual_Before
            {
                Id = 1,
                Name = "A",
                Surname = "B"
            };
            var partnerRepository = new PartnerRepository_Before();

            var result = partnerRepository.GenerateReferenceNumber(partner);
            
            Assert.AreEqual("1AB", result);
        }

        [Test]
        public void GivenPartnerRepository_After_WhenGenerateReferenceNumberForIndividual_CorrectIdIsReturned()
        {
            var partner = new Individual_After
            {
                Id = 1,
                Name = "A",
                Surname = "B"
            };
            var partnerRepository = new PartnerRepository_After();

            var result = partnerRepository.GenerateReferenceNumber(partner);

            Assert.AreEqual("1AB", result);
        }
    }

    #region Example 1

    /*
        Method uses control statements to determine the object type. 
        Method is not closed to modification.
    */
    public class PartnerRepository_Before
    {
        public string GenerateReferenceNumber(Partner_Before partner)
        {
            if(partner is Company_Before)
            {
                var company = partner as Company_Before;
                return string.Format("{0}{1}", company.Id, company.Title);
            }
            else if (partner is Individual_Before)
            {
                var individual = partner as Individual_Before;
                return string.Format("{0}{1}{2}", individual.Id, individual.Name, individual.Surname);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }

    /*
        The same result is achieved using polymorphism.
        Method conforms to the "Open and Closed" principle.
    */
    public class PartnerRepository_After
    {
        public string GenerateReferenceNumber(Partner_After partner)
        {
            return partner.GetReferenceNumber();
        }
    }

    #endregion

    #region Test classes

    public class Partner_Before
    {
        public int Id { get; set; }
    }

    public class Company_Before : Partner_Before
    {
        public string Title { get; set; }
    }

    public class Individual_Before : Partner_Before
    {
        public string Name { get; set; }
        public object Surname { get; internal set; }
    }


    public abstract class Partner_After
    {
        public int Id { get; set; }
        public abstract string GetReferenceNumber();
    }

    public class Company_After : Partner_After
    {
        public string Title { get; set; }
        public override string GetReferenceNumber()
        {
            return string.Format("{0}{1}", Id, Title);
        }
    }

    public class Individual_After : Partner_After
    {
        public string Name { get; set; }
        public object Surname { get; internal set; }
        public override string GetReferenceNumber()
        {
            return string.Format("{0}{1}{2}", Id, Name, Surname);
        }
    }

    #endregion

}

