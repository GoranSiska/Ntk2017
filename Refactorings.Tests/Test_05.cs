using NUnit.Framework;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_05 //Introduce Parameter Object
    {
        [Test]
        public void GivenPremiumCalculator_Revisited_Before_WhenCalculatePremiumOnAboveRiskAgeLowerBoundarySmokerBikeOwner_ReturnsAmmountWithVeryHighRiskInsuranceMultiplier()
        {
            var premiumCalculator = new PremiumCalculator_Revisited_Before();
            var ageAboveRiskAgeLowerBoundary = 35;

            var result = premiumCalculator.CalculatePremium(ageAboveRiskAgeLowerBoundary, true, true, 10000);

            Assert.AreEqual(20000, result);
        }

        [Test]
        public void GivenPremiumCalculator_Revisited_After_WhenCalculatePremiumOnAboveRiskAgeLowerBoundarySmokerBikeOwner_ReturnsAmmountWithVeryHighRiskInsuranceMultiplier()
        {
            var policyHolderInfo = new PolicyHolderInfo
            {
                Age = 35,
                IsSmoker = true,
                OwnsMotorcycle = true
            };
            var premiumCalculator = new PremiumCalculator_Revisited_After();


            var result = premiumCalculator.CalculatePremium(policyHolderInfo, 10000);

            Assert.AreEqual(20000, result);
        }
    }

    #region Example 1

    /*
        Wide parameter list opens up possibility for confusion. 
        Eg: CalculatePremium (35, true, false, 10000) vs CalculatePremium(35, false, true, 10000)
    */
    public class PremiumCalculator_Revisited_Before
    {
        const int RiskAgeLowerBoundary = 18;
        const decimal VeryHighRiskInsuranceMultiplier = 2m;
        const decimal HighRiskInsuranceMultiplier = 1.5m;
        const decimal MediumRiskInsuranceMultiplier = 1.3m;
        const decimal LowRiskInsuranceMultiplier = 1.1m;
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

    /*
        Parameter list is shorter and more readable.
        Parameter object prevents errors arising from incorrect order of primitive type parameters.
    */
    public class PremiumCalculator_Revisited_After
    {
        const int RiskAgeLowerBoundary = 18;
        const decimal VeryHighRiskInsuranceMultiplier = 2m;
        const decimal HighRiskInsuranceMultiplier = 1.5m;
        const decimal MediumRiskInsuranceMultiplier = 1.3m;
        const decimal LowRiskInsuranceMultiplier = 1.1m;

        public decimal CalculatePremium(PolicyHolderInfo policyHolderInfo, decimal insuranceSum)
        {
            if (policyHolderInfo.Age > RiskAgeLowerBoundary && policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle)
            {
                return insuranceSum * VeryHighRiskInsuranceMultiplier;
            }
            else if (policyHolderInfo.Age > RiskAgeLowerBoundary && policyHolderInfo.IsSmoker && !policyHolderInfo.OwnsMotorcycle)
            {
                return insuranceSum * HighRiskInsuranceMultiplier;
            }
            else if (policyHolderInfo.Age > RiskAgeLowerBoundary && !policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle)
            {
                return insuranceSum * MediumRiskInsuranceMultiplier;
            }
            else if (policyHolderInfo.Age > RiskAgeLowerBoundary && !policyHolderInfo.IsSmoker && !policyHolderInfo.OwnsMotorcycle)
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

