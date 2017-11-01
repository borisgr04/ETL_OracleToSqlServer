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
    #region LoadToOracle

    public class ExtraerFromDatabase : EtlProcess
    {
        protected override void Initialize()
        {
            Register(new ReadVigenciasOracle());
            Register(new PutConsole());
        }
    }
    public class ReadVigenciasOracle : ConventionInputCommandOperation
    {
        public ReadVigenciasOracle() : base("OracleDbContext")
        {
            Command = "select Year_Vig from vigencias";
        }
    }
    public class PutConsole : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
            {
                Console.WriteLine(row["Year_Vig"]);
            }
            yield break;
        }
    }
    #endregion
}
