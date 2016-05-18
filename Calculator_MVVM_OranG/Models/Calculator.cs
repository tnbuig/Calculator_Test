using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_MVVM_OranG.Models
{
    public class Calculator
    {
        public double Calc(double a, int b, string operation)
        {
            switch (operation)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return
                        a * b;
                case "/":
                    return a / b;
                default:
                    throw new InvalidOperationException(string.Format(
                        @"the operation requested does not available for this calculator 
\n please extend calculator abilities or choose a different operation \n 
the operation requested is : {0}", operation));
            }
        }

        public int Fib(int p)
        {
            if (p == 0 || p == 1)
                return p;
            return Fib(p - 1) + Fib(p - 2);
        }
    }

}
