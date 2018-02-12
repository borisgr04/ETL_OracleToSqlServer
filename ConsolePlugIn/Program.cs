using IPluggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlugIn
{
    class Program
    {
        static void Main(string[] args)
        {
            int 
            x = 34,
            y = 56;

            Console.WriteLine(String.Format("x={0} y={1}", x.ToString(), y.ToString()));

            foreach (CalculatorHost calculator in CalculatorHostProvider.Calculators)
            {
                calculator.X = x;
                calculator.Y = y;
                Console.WriteLine(calculator.ToString());
            }
            Console.ReadLine();
        }
    }
}
