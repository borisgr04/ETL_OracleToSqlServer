using HelloRhinoEtl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloRhinoEtl.Tests
{
    [TestFixture]
    public class TestFromOracleToSqlServerClass
    {
        [Test]
        public void TestFromOracleToSqlServer()
        {
            FromOracleToSqlServer etl =new FromOracleToSqlServer();
            etl.Execute();
            foreach (var item in etl.GetAllErrors())
            {
                Console.WriteLine(item.Message);
            }
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }
    }
}
