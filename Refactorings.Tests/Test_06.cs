using NUnit.Framework;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_06 //Replace Magic Numbers with Symbols
    {
        [Test]
        public void GivenPremiumCalculator_Before_WhenCalculatePremiumOnOlderSmokerBikeOwner_ReturnsCorrectAmmount()
        {
            var premiumCalculator = new PremiumCalculator_Before();

            var result = premiumCalculator.CalculatePremium(35, true, true, 10000);

            Assert.AreEqual(20000, result);
        }

        [Test]
        public void GivenPremiumCalculator_After_WhenCalculatePremiumOnAboveRiskAgeLowerBoundarySmokerBikeOwner_ReturnsAmmountWithVeryHighRiskInsuranceMultiplier()
        {
            //TODO: split tests
            var ageAboveRiskAgeLowerBoundary = 35;

            Assert.Greater(ageAboveRiskAgeLowerBoundary, PremiumCalculator_After.RiskAgeLowerBoundary);

            var premiumCalculator = new PremiumCalculator_After();

            var result = premiumCalculator.CalculatePremium(ageAboveRiskAgeLowerBoundary, true, true, 10000);

            Assert.AreEqual(20000, result);
        }
    }

    #region Example 1

    /*
        Extensive use of "magic numbers".
        High potential for error.
        Low readability.
    */
    public class PremiumCalculator_Before
    {
        public decimal CalculatePremium(int age, bool isSmoker, bool ownsMotorcycle, decimal insuranceSum)
        {
            if (age > 18 && isSmoker && ownsMotorcycle)
            {
                return insuranceSum * 2;
            }
            else if (age > 18 && isSmoker && !ownsMotorcycle)
            {
                return insuranceSum * 1.3m;
            }
            else if (age > 18 && !isSmoker && ownsMotorcycle)
            {
                return insuranceSum * 1.5m;
            }
            else if (age > 18 && !isSmoker && !ownsMotorcycle)
            {
                return insuranceSum * 1.1m;
            }
            else
            {
                return insuranceSum;
            }
        }
    }

    /*
        Business constants are localized / exposed.
        Lower potential for error.
        Higher readability.
    */
    public class PremiumCalculator_After
    {
        public const int RiskAgeLowerBoundary = 18;
        public const decimal VeryHighRiskInsuranceMultiplier = 2m;
        public const decimal HighRiskInsuranceMultiplier = 1.5m;
        public const decimal MediumRiskInsuranceMultiplier = 1.3m;
        public const decimal LowRiskInsuranceMultiplier = 1.1m;

        public decimal CalculatePremium(int age, bool isSmoker, bool ownsMotorcycle, decimal insuranceSum)
        {
            if (age > RiskAgeLowerBoundary && isSmoker && ownsMotorcycle)
            {
                return insuranceSum * VeryHighRiskInsuranceMultiplier;
            }
            else if (age > RiskAgeLowerBoundary && isSmoker && !ownsMotorcycle)
            {
                return insuranceSum * HighRiskInsuranceMultiplier;
            }
            else if (age > RiskAgeLowerBoundary && !isSmoker && ownsMotorcycle)
            {
                return insuranceSum * MediumRiskInsuranceMultiplier;
            }
            else if (age > RiskAgeLowerBoundary && !isSmoker && !ownsMotorcycle)
            {
                return insuranceSum * LowRiskInsuranceMultiplier;
            }
            else
            {
                return insuranceSum;
            }
        }
    }

    #endregion
}

