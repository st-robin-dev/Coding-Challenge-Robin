using StorageModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace StorageModule.Database
{
    internal class DatabaseFactory
    {
        public IDatabaseContext Build(bool skipValidation = false)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Authentication"].ToString();
            var sqlConnection = new SqlConnection(connectionString);
            var context = new DatabaseContext(sqlConnection, skipValidation);
            context.Database.CreateIfNotExists();
            context.Database.Initialize(false);
            return context;
        }
    }
}
