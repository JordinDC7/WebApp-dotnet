using System.Data;
using System.Data.SqlClient;
using RockShow.Interfaces;
namespace RockShow.Data
{
    public class SqlConnectionCreator : IDbConnectionCreator
    {
        private readonly string _connectionString;

        public SqlConnectionCreator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
