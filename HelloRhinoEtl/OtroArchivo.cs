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
    public static class InicializarDatosEF {
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
    public static class AdoNet {
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
    /// <summary>
    /// The Data Class that represent each row. Notice the DelimetedRecord annotation. That is from the File Helper
    /// </summary>
    [DelimitedRecord(",")]
    public class DataRecord
    {
        public int Id;
        public string AWord;
    }
    /// <summary>
    /// Just get data from a File. Could be database or fake constructed data
    /// </summary>
    public class SimpleFileDataGet : AbstractOperation
    {
        public SimpleFileDataGet(string inPutFilepath)
        {
            FilePath = inPutFilepath;
        }
        public string FilePath { get; set; }
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {

            using (FileEngine file = FluentFile.For<DataRecord>().From(FilePath))
            {
                foreach (object obj in file)
                {
                    yield return Row.FromObject(obj);
                }
            }
        }
    }

    public class TransformWord : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
            {
                var revWord = (string)row["AWord"];
                row["AWord"] = new string(revWord.ToCharArray().Reverse().ToArray());
                yield return row;
            }
        }
    }

    public class JoinWordLists : JoinOperation
    {
        protected override void SetupJoinConditions()
        {
            InnerJoin
                .Left("Id")
                .Right("Id");
        }

        protected override Row MergeRows(Row leftRow, Row rightRow)
        {
            Row row = leftRow.Clone();
            row["AWord"] = leftRow["AWord"].ToString() + " " +
                                       rightRow["AWord"].ToString();
            return row;
        }
    }

    /// <summary>
    /// We will just put data on the screen. Would be more realistic to put to other file or database
    /// </summary>
    public class PutData : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
            {
                var record = new DataRecord
                {
                    Id = (int)row["Id"],
                    AWord = (string)row["AWord"]
                };
                Console.WriteLine(record.AWord);
            }
            yield break;
        }
    }
    /// <summary>
    /// Here is the actual ETL process where all steps are registred. 
    /// It represent one dataflow, We get two datasources Join them, Transform data and put it somewhere.
    /// </summary>
    public class ExNihiloProcess : EtlProcess
    {
        protected override void Initialize()
        {    // my path to the file is D:\Users\Patrik\Documents\GitHub\HelloWorld-Rhino-ETL\HelloRhinoEtl\UntransformedWordList.csv
            //Relative Path is for me : ..\..\..\UntransformedWordList1.csv
            //A hash join operation between the files on the id
            Register(new JoinWordLists()
                .Left(new SimpleFileDataGet(@"..\..\..\UntransformedWordList1.csv"))
                .Right(new SimpleFileDataGet(@"..\..\..\UntransformedWordList2.csv"))
                );
            // A silly Transformation of each row
            Register(new TransformWord());
            //Put the data on the screen. Should normally be file or database table
            Register(new PutData());
        }
    }

    
    public class BatchUpdateUserNames : SqlBatchOperation
    {
        public BatchUpdateUserNames()
            : base("Test")
        {
        }

        /// <summary>
        /// Prepares the command from the given row
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="command">The command.</param>
        protected override void PrepareCommand(Row row, SqlCommand command)
        {
            command.CommandText = "UPDATE Users SET Name = @Name, TestMsg = 'UpperCased' WHERE Id = @Id";
            command.Parameters.AddWithValue("@Name", row["Name"]);
            command.Parameters.AddWithValue("@Id", row["Id"]);
        }
    }
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




}
