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
    #region ETLActividades
    public class FromSqlServerJoinOracleToSqlServer : EtlProcess
    {
        protected override void Initialize()
        {
            Register(
                    new JoinActividadVigencia()
                    .Left(new ExtraerActividadesFromOracle())
                    .Right(new ExtraerVigenciaFromSqlServer())
                    );
            
            Register(new LoadActividadesToSqlServer());
        }
    }

    public class JoinActividadVigencia : JoinOperation
    {
        
        protected override void SetupJoinConditions()
        {
            InnerJoin
                .Left("Vigencia")
                .Right("Year");
        }

        protected override Row MergeRows(Row leftRow, Row rightRow)
        {
            Row row = leftRow.Clone();
            row["Vigencia_VigenciaId"] = rightRow["VigenciaId"].ToString();
            return row;
        }
    }

    public class ExtraerActividadesFromOracle : ConventionInputCommandOperation
    {
        public ExtraerActividadesFromOracle() : base("OracleDbContext")
        {
            Command = "SELECT Nom_Act,Vigencia,Estado FROM PActividades";
        }
    }

    public class ExtraerVigenciaFromSqlServer : ConventionInputCommandOperation
    {
        public ExtraerVigenciaFromSqlServer() : base("Test")
        {
            Command = "select Year,VigenciaId from Vigencias";
        }
    }

   
    public class TransformActividadesData : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
            {
                //var revWord = row["Year_Vig"].ToString();
                //row["Year"] = new string(revWord.ToCharArray().Reverse().ToArray());
                //row["Estado"] = "AC";
                yield return row;
            }
        }
    }

    public class LoadActividadesToSqlServer : ConventionOutputCommandOperation
    {
        public LoadActividadesToSqlServer() : base("Test")
        {
            Command = "INSERT INTO Actividads (Nombre,Estado,Vigencia_VigenciaId) VALUES(@Nom_Act,@Estado,@Vigencia_VigenciaId)";
        }
    }

    #endregion
  
    #region ETLVigencia

    public class FromOracleToSqlServer : EtlProcess
    {
        readonly int IdInicial=0;
        public FromOracleToSqlServer(int idInicial) {
            IdInicial = idInicial;
        }
        protected override void Initialize()
        {
            Register(new ExtraerVigenciaFromOracle())
                .Register(new TransformData(IdInicial))
                .Register(new LoadToSqlServer());
        }
    }
    public class ExtraerVigenciaFromOracle : ConventionInputCommandOperation
    {
        public ExtraerVigenciaFromOracle() : base("OracleDbContext")
        {
            Command = "select Year_Vig from vigencias";
        }
    }
    public class TransformData : AbstractOperation
    {
        private int IdInicial;

        public TransformData(int idInicial)
        {
            IdInicial = idInicial;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (Row row in rows)
            {
                var revWord = row["Year_Vig"].ToString();
                row["Year"] = new string(revWord.ToCharArray().Reverse().ToArray());
                row["Estado"] = "AC";
                row["IdCalculado"] = IdInicial++;
                yield return row;
            }
        }
    }
    public class LoadToSqlServer : ConventionOutputCommandOperation
    {
        public LoadToSqlServer() : base("Test")
        {
            Command = "INSERT INTO Vigencias (Year,Estado,IdCalculado) VALUES(@Year,@Estado,@IdCalculado)";
        }
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            var Command = "ConsultarDB";
            Console.WriteLine("----Lets create a Rhino-ETL ----");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(Command);
            if (Command == "OracleToSql")
            {
                Execute(new FromOracleToSqlServer(10));
            }
            if (Command == "JoinFile")
            {
                Execute(new JoinFileProcess());
            }
            if (Command == "InicializarTestDB")
            {
                InicializarDatosEF.InicializarDatos();
            }
            if (Command == "ConsultarDB")
            {
               Console.WriteLine("Reg:" + InicializarDatosEF.ConsultarDatos());
            }
            if (Command == "JoinBD")
            {
                Execute(new FromSqlServerJoinOracleToSqlServer());
            }
            
            Console.WriteLine("-------------------------------");
            Console.WriteLine("----Hit any Rhino to exit------");
            Console.ReadKey();
        }

        private static void Execute(EtlProcess etl )
        {
            using (etl)
            {
                etl.Execute();
                foreach (var item in etl.GetAllErrors())
                {
                    Console.WriteLine(item.Message);
                }
            }
            
        }
    }
    
 
}
