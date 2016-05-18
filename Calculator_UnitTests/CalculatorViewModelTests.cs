using System;
using Calculator_MVVM_OranG.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;

namespace Calculator_UnitTests
{
    //with this unit test class/project 
    //just wanted to show best practice i do not really have time to write unit tests
    [TestClass]
    public class CalculatorViewModelTests
    {
        [TestMethod]
        public void InsertInputViewModel()
        {
            CalculatorViewModel calcViewModel = new CalculatorViewModel();
            ICommand digitCommand = calcViewModel.DigitKeyCommand;
            digitCommand.Execute("1");
            Assert.AreEqual(calcViewModel.Input, "1","input command did not changed the input as expected");
        }
    }
}
