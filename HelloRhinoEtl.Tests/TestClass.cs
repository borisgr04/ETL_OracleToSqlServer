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
            FromOracleToSqlServer etl =new FromOracleToSqlServer(1);
            etl.Execute();
            foreach (var item in etl.GetAllErrors())
            {
                Console.WriteLine(item.Message);
            }
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }
        [Test]
        public void TestConsultaCount()
        {
            Assert.Greater(InicializarDatosEF.ConsultarDatos(), 350000);
        }
        [Test]
        public void TestConsultarVigencia()
        {
            Vigencia v = InicializarDatosEF.ConsultarVigencia("100000");
            Assert.NotNull(v);
            Assert.AreEqual(v.Year, "100000");
        }
        [Test]
        public void TestConsultarAllVigencia()
        {
            Assert.AreEqual(InicializarDatosEF.ConsultarAllVigencia("0").Count,1817628);
        }
    }
}
