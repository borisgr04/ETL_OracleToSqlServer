using IPluggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiplicar
{
    [CalculationPlugIn("This plug-in will add two numbers together")]
    class Mult : ICalculator
    {
        #region ICalculator Members

        public int Calculate(int a, int b)
        {
            return a * b;
        }

        public char GetSymbol()
        {
            return '*';
        }

        #endregion

    }
}
