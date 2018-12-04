using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Domain.Repositories
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString() 
        {
            return GetConnectionString("default");
        }

        public string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }
    }
}
