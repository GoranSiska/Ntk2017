using NSubstitute;
using NUnit.Framework;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_04 //Replace Expressions with Methods 
    {
        [Test]
        public void GivenPremiumCalculator_Revisited_Again_Before_WhenCalculatePremiumOnAboveRiskAgeLowerBoundarySmokerBikeOwner_ReturnsAmmountWithVeryHighRiskInsuranceMultiplier()
        {
            var policyHolderInfo = new PolicyHolderInfo
            {
                Age = 35,
                IsSmoker = true,
                OwnsMotorcycle = true
            };
            var premiumCalculator = new PremiumCalculator_Revisited_Again_Before();

            var result = premiumCalculator.CalculatePremium(policyHolderInfo, 10000);

            Assert.AreEqual(20000, result);
        }

        [Test]
        public void GivenPremiumCalculator_Revisited_Again_After_WhenIsVeryHighRisk_ReturnsTrueForHighRiskPolicyHolders()
        {
            var policyHolderInfo = new PolicyHolderInfo
            {
                Age = 35,
                IsSmoker = true,
                OwnsMotorcycle = true
            };
            var premiumCalculator = new PremiumCalculator_Revisited_Again_After();

            var result = premiumCalculator.IsVeryHighRisk(policyHolderInfo);

            Assert.IsTrue(result);
        }

        [Test]
        public void GivenPremiumCalculator_Revisited_Again_After_WithVeryHighRiskPolicyHolder_ReturnsAmmountWithVeryHighRiskInsuranceMultiplier()
        {
            var premiumCalculator = Substitute.For<PremiumCalculator_Revisited_Again_After>();
            premiumCalculator.IsVeryHighRisk(Arg.Any<PolicyHolderInfo>()).Returns(true);

            var result = premiumCalculator.CalculatePremium(new PolicyHolderInfo(), 10000);

            Assert.AreEqual(20000, result);
        }
    }

    #region Example 1

    /*
        Control statement expressions use multiple values and are hard to read.
        Control statement logic is tested indirectly.
        Control statement logic is part of calculation tests.
    */
    public class PremiumCalculator_Revisited_Again_Before
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

    /*
        Control statement expressions are abstracted and correctly named. 
        Can be read and understood in isolation instead of inline with calculation.
        Risk logic can be tested directly.
        Risk logic and calculation logic can be tested independently.
    */
    public class PremiumCalculator_Revisited_Again_After
    {
        const int RiskAgeLowerBoundary = 18;
        const decimal VeryHighRiskInsuranceMultiplier = 2m;
        readonly decimal HighRiskInsuranceMultiplier = 1.5m;
        const decimal MediumRiskInsuranceMultiplier = 1.3m;
        const decimal LowRiskInsuranceMultiplier = 1.1m;
        public decimal CalculatePremium(PolicyHolderInfo policyHolderInfo, decimal insuranceSum)
        {
            if (IsVeryHighRisk(policyHolderInfo))
            {
                return insuranceSum * VeryHighRiskInsuranceMultiplier;
            }
            else if (IsHighRisk(policyHolderInfo))
            {
                return insuranceSum * HighRiskInsuranceMultiplier;
            }
            else if (IsMediumRisk(policyHolderInfo))
            {
                return insuranceSum * MediumRiskInsuranceMultiplier;
            }
            else if (IsLowRisk(policyHolderInfo))
            {
                return insuranceSum * LowRiskInsuranceMultiplier;
            }
            else // IsNoRisk
            {
                return insuranceSum;
            }
        }

        public virtual bool IsVeryHighRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > RiskAgeLowerBoundary && policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle;
        }

        public virtual bool IsHighRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > RiskAgeLowerBoundary && !policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle;
        }

        public virtual bool IsMediumRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > RiskAgeLowerBoundary && policyHolderInfo.IsSmoker && !policyHolderInfo.OwnsMotorcycle;
        }

        public virtual bool IsLowRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > RiskAgeLowerBoundary && !policyHolderInfo.IsSmoker && !policyHolderInfo.OwnsMotorcycle;
        }
    }

    #endregion
}

