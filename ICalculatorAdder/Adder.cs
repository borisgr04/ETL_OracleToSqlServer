using IPluggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPluggable
{
    [CalculationPlugIn("This plug-in will add two numbers together")]
    class Adder : ICalculator
    {
        #region ICalculator Members

        public int Calculate(int a, int b)
        {
            return a + b;
        }

        public char GetSymbol()
        {
            return '+';
        }

        #endregion

    }
}
