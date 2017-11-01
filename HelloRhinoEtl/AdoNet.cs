using Rhino.Etl.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloRhinoEtl
{
    public static class AdoNet
    {
        private static int GetUserCount(string where)
        {
            return Use.Transaction<int>("Test", delegate (IDbCommand command)
            {
                command.CommandText = "select count(*) from users where " + where;
                return (int)command.ExecuteScalar();
            });
        }
        static void AdoNetExample()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TestETL;Integrated Security=SSPI";
            string queryString = "SELECT * from users ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }
    }

}
