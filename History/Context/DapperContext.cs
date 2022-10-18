using System.Data;
using System.Data.SqlClient;

namespace History.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string sqlconnection;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlconnection = _configuration.GetConnectionString("sqlConnection");
        }
        public IDbConnection CreateConnection() => new SqlConnection(sqlconnection);
    }
}
