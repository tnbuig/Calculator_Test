using System;
using Calculator_MVVM_OranG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator_UnitTests
{
    //with this unit test class/project 
    //just wanted to show best practice i do not really have time to write unit tests
    [TestClass]
    public class CalculatorModelTests
    {
        [TestMethod]
        public void CalculatePositiveSum()
        {
            Calculator calc = new Calculator();
            double result = calc.Calc(10, 35, "+");
            Assert.AreEqual(result, 45, "result of sum operation is not as expected for in");
        }

        [TestMethod]
        public void CalcuatedPositiveSub()
        {
            Calculator calc = new Calculator();
            double result = calc.Calc(10, 10, "-");
            Assert.AreEqual(result, 0, "result of subtract operation is not as expected for in");
        }

        

    }
}
