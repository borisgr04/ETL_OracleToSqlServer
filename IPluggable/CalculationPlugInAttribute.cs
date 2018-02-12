using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPluggable
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CalculationPlugInAttribute : Attribute
    {
        public string Description { get; set; }

        public CalculationPlugInAttribute(string description)
        {
            Description = description;
        }
    }
}
