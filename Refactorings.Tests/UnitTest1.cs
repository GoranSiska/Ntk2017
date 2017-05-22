using Moq;
using NUnit.Framework;
using System;
using System.Collections;

namespace Refactorings.Tests
{
    [TestFixture]
    public class Refactorings
    {
        [Test]
        public void MoveBooleanExpressionToBooleanFunction()
        {
            //var calculator = new PremiumCalculator();
            //var calculator = new PremiumCalculatorWithReducedParameters();
            //var calculator = new PremiumCalculatorWithBooleanConditionMethods();
            var calculator = new PremiumCalculatorWithDAT();
            var expectedPremium = 200m;

            var result = calculator.CalculatePremium(32, true, true, 100m);
            //var policyHolderInfo = new PolicyHolderInfo (32, true, true );
            //var result = calculator.CalculatePremium(policyHolderInfo, 100m);

            Assert.AreEqual(expectedPremium, result);
        }

        [Test]
        public void MoveBooleanExpressionToBooleanFunctionMock()
        {
            var calculatorMock = new Mock<PremiumCalculatorWithBooleanConditionMethods>();
            calculatorMock.CallBase = true;
            calculatorMock.Setup(c => c.IsVeryHighRisk(It.IsAny<PolicyHolderInfo>())).Returns(true);
            var calculator = calculatorMock.Object;
            var expectedPremium = 200m;

            var result = calculator.CalculatePremium(null, 100m);

            Assert.AreEqual(expectedPremium, result);
        }
    }

    public class PremiumCalculator
    {
        public decimal CalculatePremium(int age, bool isSmoker, bool ownsMotorcycle, decimal insuranceSum)
        {
            if(age > 18 && isSmoker && ownsMotorcycle)
            {
                return insuranceSum * 2;
            }
            else if (age > 18 && isSmoker && !ownsMotorcycle)
            {
                return insuranceSum * 1.3m;
            }
            else if(age > 18 && !isSmoker && ownsMotorcycle)
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

    public class PremiumCalculatorWithReducedParameters
    {
        public decimal CalculatePremium(PolicyHolderInfo policyHolderInfo, decimal insuranceSum)
        {
            if(policyHolderInfo == null)
            {
                return insuranceSum;
            }

            if (policyHolderInfo.Age > 18 && policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle)
            {
                return insuranceSum * 2;
            }
            else if (policyHolderInfo.Age > 18 && !policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle)
            {
                return insuranceSum * 1.5m;
            }
            else //policyHolderInfo.Age < 18
            {
                return insuranceSum;
            }
        }
    }



    public class PolicyHolderInfo
    {
        private int _age;
        private bool _isSmoker;
        private bool _ownsMotorcycle;

        public PolicyHolderInfo(int age, bool isSmoker, bool ownsMotorcycle)
        {
            _age = age;
            _isSmoker = isSmoker;
            _ownsMotorcycle = ownsMotorcycle;
        }

        public int Age { get { return _age; } private set { _age = value; } }
        public bool IsSmoker { get { return _isSmoker; } private set { _isSmoker = value; } }
        public bool OwnsMotorcycle { get { return _ownsMotorcycle; } private set { _ownsMotorcycle = value; } }
    }

    public class NullPolicyHolderInfo : PolicyHolderInfo
    {
        public NullPolicyHolderInfo() : base(0, false, false)
        { }
    }

    public class PremiumCalculatorWithBooleanConditionMethods
    {
        public decimal CalculatePremium(PolicyHolderInfo policyHolderInfo, decimal insuranceSum)
        {
            //if (policyHolderInfo == null)
            //{
            //    return insuranceSum;
            //}

            if (IsVeryHighRisk(policyHolderInfo))
            {
                return insuranceSum * 2;
            }
            else if (IsHighRisk(policyHolderInfo))
            {
                return insuranceSum * 1.5m;
            }
            else if (IsMediumRisk(policyHolderInfo))
            {
                return insuranceSum * 1.3m;
            }
            else if (IsLowRisk(policyHolderInfo))
            {
                return insuranceSum * 1.1m;
            }
            else // IsNoRisk
            {
                return insuranceSum;
            }
        }

        public virtual bool IsVeryHighRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > 18 && policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle;
        }

        public virtual bool IsHighRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > 18 && !policyHolderInfo.IsSmoker && policyHolderInfo.OwnsMotorcycle;
        }

        public virtual bool IsMediumRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > 18 && policyHolderInfo.IsSmoker && !policyHolderInfo.OwnsMotorcycle;
        }

        public virtual bool IsLowRisk(PolicyHolderInfo policyHolderInfo)
        {
            return policyHolderInfo.Age > 18 && !policyHolderInfo.IsSmoker && !policyHolderInfo.OwnsMotorcycle;
        }
    }


    public class PremiumCalculatorWithDAT
    {
        decimal[,,] decisionMatrix = new decimal[2,2,2]
        {
            { //!isOver18
                { //!isSmoker
                    1m, //!ownsMotorcycle
                    1m //ownsMotorcycle
                },
                { //isSmoker
                    1m, //!ownsMotorcycle
                    1m //ownsMotorcycle
                }
            },
            { //isOver18
                { //!isSmoker
                    1.1m, //!ownsMotorcycle
                    1.5m //ownsMotorcycle
                },
                { //isSmoker
                    1.3m, //!ownsMotorcycle
                    2m //ownsMotorcycle
                }
            }
        };
        public decimal CalculatePremium(int age, bool isSmoker, bool ownsMotorcycle, decimal insuranceSum)
        {
            var isOver18Dimension = Convert.ToInt32(age > 18);
            var isSmokerDimension = Convert.ToInt32(isSmoker);
            var ownsMotorcycleDimension = Convert.ToInt32(ownsMotorcycle);

            return insuranceSum * decisionMatrix[isOver18Dimension, isSmokerDimension, ownsMotorcycleDimension];
        }
    }

    public class PremiumCalculatorWithConstants
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
}
