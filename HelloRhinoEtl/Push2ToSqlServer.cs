using Rhino.Etl.Core;
using Rhino.Etl.Core.ConventionOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloRhinoEtl
{
    #region Push2ToSqlServer
    public class PushData2ToDatabase : EtlProcess
    {
        public PushData2ToDatabase(int expectedCount)
        {
            this.expectedCount = expectedCount;
        }

        private readonly int expectedCount;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            Register(new ReadUsers());
            Register(new InsertOutput());
        }
    }
    public class ReadUsers : ConventionInputCommandOperation
    {
        public ReadUsers() : base("Test")
        {
            Command = "SELECT UserId, Name,Email FROM Users";
        }
    }
    public class InsertOutput : ConventionOutputCommandOperation
    {
        public InsertOutput() : base("Test")
        {
            Command = "INSERT INTO Users (Name,Email) VALUES(@Name,@Email)";
        }
    }
    #endregion
}
