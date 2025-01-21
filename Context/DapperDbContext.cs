using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication8.Context
{
    public class DapperDbContext
    {
        private readonly string _connectionString;

        public DapperDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
