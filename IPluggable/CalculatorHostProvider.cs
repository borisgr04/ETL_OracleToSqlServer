using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IPluggable
{
    public static class CalculatorHostProvider
    {

        private static List<CalculatorHost> m_calculators;

        public static List<CalculatorHost> Calculators
        {
            get
            {
                if (null == m_calculators)
                    Reload();

                return m_calculators;
            }
        }

        public static void Reload()
        {

            if (null == m_calculators)
                m_calculators = new List<CalculatorHost>();
            else
                m_calculators.Clear();

            m_calculators.Add(new CalculatorHost()); // load the default
            List<Assembly> plugInAssemblies = LoadPlugInAssemblies();
            List<ICalculator> plugIns = GetPlugIns(plugInAssemblies);

            foreach (ICalculator calc in plugIns)
            {
                m_calculators.Add(new CalculatorHost(calc));
            }
        }
        private static List<Assembly> LoadPlugInAssemblies()
        {
            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
            FileInfo[] files = dInfo.GetFiles("*.dll");
            List<Assembly> plugInAssemblyList = new List<Assembly>();

            if (null != files)
            {
                foreach (FileInfo file in files)
                {
                    plugInAssemblyList.Add(Assembly.LoadFile(file.FullName));
                }
            }

            return plugInAssemblyList;

        }

        static List<ICalculator> GetPlugIns(List<Assembly> assemblies)
        {
            List<Type> availableTypes = new List<Type>();

            foreach (Assembly currentAssembly in assemblies)
                availableTypes.AddRange(currentAssembly.GetTypes());

            // get a list of objects that implement the ICalculator interface AND 
            // have the CalculationPlugInAttribute
            List<Type> calculatorList = availableTypes.FindAll(delegate (Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                object[] arr = t.GetCustomAttributes(typeof(CalculationPlugInAttribute), true);
                return !(arr == null || arr.Length == 0) && interfaceTypes.Contains(typeof(ICalculator));
            });

            // convert the list of Objects to an instantiated list of ICalculators
            return calculatorList.ConvertAll<ICalculator>(delegate (Type t) { return Activator.CreateInstance(t) as ICalculator; });

        }
    }
}
