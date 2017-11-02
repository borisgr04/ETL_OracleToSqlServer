using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloRhinoEtl
{
    public class Vigencia
    {
        public int VigenciaId { get; set; }
        public string Year { get; set; }
        public string Estado { get; set; }
        public ICollection<Actividad> Actividades { get; set; }
    }
    public class Actividad
    {
        public int ActividadId { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
    }
    public class TestContext : DbContext
    {
        public TestContext()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestContext>());
        }
        public DbSet<Vigencia> Vigencias { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        
    }

    public static class InicializarDatosEF
    {
        public static void InicializarDatos()
        {
            #region MyRegion
            using (TestContext db = new TestContext())
            {
                string[] Vigs = { "1990", "1991", "1992" };
                foreach (var item in Vigs)
                {
                    db.Vigencias.Add(new Vigencia()
                                     {   
                                        Year = item ,
                                        Estado = "IN",
                                        Actividades = new List<Actividad>()
                                        {
                                            new Actividad() { Nombre="Prueba",Estado="AC" }
                                        }
                                    }
                                );
                }
                int i = db.SaveChanges();
                Console.WriteLine("Reg" + i.ToString());
                Console.ReadKey();
            }
            #endregion
        }
    }
}
