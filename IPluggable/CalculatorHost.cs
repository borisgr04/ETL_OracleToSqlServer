using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPluggable
{
    public class CalculatorHost
    {
        private readonly ICalculator m_calculator;

        public CalculatorHost(ICalculator calculator)
        {
            m_calculator = calculator;
        }

        public CalculatorHost() : this(new Divider()) { }
        
        public int X { get; set; }

        public int Y { get; set; }

        public int Calculate()
        {
            return m_calculator.Calculate(X, Y);
        }

        public override string ToString()
        {
            return $"{X} {m_calculator.GetSymbol()} {Y} = {Calculate()}";
        }

    }
}
