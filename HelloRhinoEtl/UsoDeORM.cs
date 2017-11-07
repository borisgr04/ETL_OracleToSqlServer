using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
           this.Database.CommandTimeout= 60*10;
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
                int nr = 0;
                for (int i=0; i<1000000; i ++)
                {
                    db.Vigencias.Add(new Vigencia()
                                     {   
                                        Year = i.ToString(),
                                        Estado = "AC"
                                    }
                                );
                    nr += db.SaveChanges();
                    if (nr % 1000==0) {
                        Console.WriteLine("Reg:" + nr.ToString());
                    }
                    
                }
                
                Console.WriteLine("Reg:" + nr.ToString());
                Console.ReadKey();
            }
            #endregion
        }

        public static int ConsultarDatos()
        {
            #region MyRegion
            using (TestContext db = new TestContext())
            {
                int r = db.Vigencias.Count();
                return r;
            }
            #endregion
        }
        public static Vigencia ConsultarVigencia(string year)
        {
            #region MyRegion
            using (TestContext db = new TestContext())
            {
                return db.Vigencias.FirstOrDefault(t => t.Year == year);
            }
            #endregion
        }
        public static List<Vigencia> ConsultarAllVigencia(string pattern)
        {
            #region MyRegion
            using (TestContext db = new TestContext())
            {
                return db.Vigencias.Where(t=>t.Year.Contains(pattern)).ToList();
            }
            #endregion
        }
    }
}
