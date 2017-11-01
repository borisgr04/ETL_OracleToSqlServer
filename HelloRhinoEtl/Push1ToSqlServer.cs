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
    #region Push1ToSqlServer

    /// <summary>
    /// Etl
    /// </summary>
    public class PushDataToDatabase : EtlProcess
    {
        public PushDataToDatabase(int expectedCount)
        {
            this.expectedCount = expectedCount;
        }

        private readonly int expectedCount;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            Register(new GenerateUsers(expectedCount));
            Register(new BulkInsertUsers());
        }
    }

    /// <summary>
    /// Genera Datos Aleatorios
    /// </summary>
    public class GenerateUsers : AbstractOperation
    {
        public GenerateUsers(int expectedCount)
        {
            this.expectedCount = expectedCount;
        }

        private int expectedCount;

        /// <summary>
        /// Executes this operation
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            for (int i = 0; i < expectedCount; i++)
            {
                Row row = new Row();
                row["id"] = i;
                row["name"] = "boris #" + i;
                row["email"] = "boris" + i + "@example.org";
                yield return row;
            }
        }
    }
    /// <summary>
    /// Insert Bulk
    /// </summary>
    public class BulkInsertUsers : SqlBulkInsertOperation
    {
        public BulkInsertUsers()
        : base("Test", "Users")
        {
        }
        /// <summary>
        /// Prepares the schema of the target table
        /// </summary>
        protected override void PrepareSchema()
        {
            Schema["Name"] = typeof(string);
            Schema["Email"] = typeof(string);
            Schema["TestMsg"] = typeof(string);
            Console.WriteLine("preparó ...");
        }


    }

    #endregion


}
