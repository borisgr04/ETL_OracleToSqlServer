using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Files;
using Rhino.Etl.Core.Operations;
using Rhino.Etl.Core.ConventionOperations;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data;
using Rhino.Etl.Core.Infrastructure;

namespace HelloRhinoEtl
{
    public class FromOracleToSqlServer : EtlProcess
    {
        protected override void Initialize()
        {
            Register(new ExtraerFromOracle())
                .Register(new TransformData())
                .Register(new LoadToSqlServer());
        }
    }
    public class ExtraerFromOracle : ConventionInputCommandOperation
    {
        public ExtraerFromOracle() : base("OracleDbContext")
        {
            Command = "select Year_Vig from vigencias";
        }
    }
    public class TransformData : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
            {
                var revWord = row["Year_Vig"].ToString();
                row["Name"] = new string(revWord.ToCharArray().Reverse().ToArray());
                row["Email"] = $"{revWord}@gmail.com";
                yield return row;
            }
        }
    }
    public class LoadToSqlServer : ConventionOutputCommandOperation
    {
        public LoadToSqlServer() : base("Test")
        {
            Command = "INSERT INTO Users (Name,Email) VALUES(@Name,@Email)";
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            LoadTest(new FromOracleToSqlServer());
        }
        private static void LoadTest(EtlProcess etl )
        {
            Console.WriteLine("----Lets create a Rhino-ETL ----");
            Console.WriteLine("--------------------------------");
            // Here is the actual work. 
            using (etl)
            {
                etl.Execute();
                foreach (var item in etl.GetAllErrors())
                {
                    Console.WriteLine(item.Message);
                }
            }
            Console.WriteLine("-------------------------------");
            Console.WriteLine("----Hit any Rhino to exit------");
            Console.ReadKey();
        }
    }
    
 
}
