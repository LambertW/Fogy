using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Domain.Uow
{
    public class DefaultDbConnectionProvider : IDbConnectionProvider
    {
        private string _connectionString;

        public DefaultDbConnectionProvider(IConnectionStringProvider provider)
        {
            _connectionString = provider.GetConnectionString();
        }

        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
