using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloRhinoEtl
{
    public class Test
    {
        private List<string> list = null;
        public Test()
        {
            Console.WriteLine("List Generated:");
            list = new List<string>() {
                "Sourav","Ram"
            };

        }
        public List<string> Names
        {
            get
            {
                return list;
            }
        }
    }
    public class ProgramLazy
    {
        readonly Lazy<Test> _Test= new Lazy<Test>();

        public Test Test
        {
            get
            {
                return _Test.Value;
            }
        }
        void Procesar()
        {
            Console.WriteLine("Data Loaded : " + _Test.IsValueCreated);

            foreach (string tmp in Test.Names)
            {
                Console.WriteLine(tmp);
            }

            Console.WriteLine("Data Loaded : " + _Test.IsValueCreated);

            Console.ReadLine();
        }
    }
}

