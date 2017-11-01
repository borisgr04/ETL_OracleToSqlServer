using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloRhinoEtl
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TestMsg { get; set; }
    }
    public class TestContext : DbContext
    {
        public TestContext()
        {

        }
        public DbSet<User> Users { get; set; }
    }

    public static class InicializarDatosEF
    {
        private static void InicializarDatos()
        {
            #region MyRegion
            using (TestContext db = new TestContext())
            {
                string[] Nombres = { "Anya", "Boris", "Arturo" };
                foreach (var item in Nombres)
                {
                    db.Users.Add(new User() { Email = item + "@gmail.com", Name = item, TestMsg = "Ok.." });
                }
                int i = db.SaveChanges();
                Console.WriteLine("Reg" + i.ToString());
                Console.ReadKey();
            }
            #endregion
        }
    }
}
