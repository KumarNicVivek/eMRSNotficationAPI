using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.DataAccessAdo
{
    public class SqlHelper : ISqlHelper, IDisposable
    {
        private readonly SqlConnection _connection;
        private SqlTransaction? _transaction;

        //private readonly string _connectionString;
        public SqlHelper(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _connection.Open();
        }

        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public SqlConnection Connection => _connection;
        public SqlTransaction? Transaction => _transaction;

        //public SqlConnection CreateOpenConnection()
        //{
        //    var connection = new SqlConnection(_connectionString);
        //    connection.Open();
        //    return connection;
        //}

        public async Task<int> ExecuteAsync(string sql, SqlParameter[]? parameters = null, SqlTransaction? transaction = null)
        {
            //using var command = new SqlCommand(sql, transaction?.Connection ?? CreateOpenConnection());
            using var command = new SqlCommand(sql, _connection, transaction);
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            command.Transaction = transaction;
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<List<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> map, SqlParameter[]? parameters = null, SqlTransaction? transaction = null)
        {
            var result = new List<T>();
            //using var command = new SqlCommand(sql, transaction?.Connection ?? CreateOpenConnection());
            using var command = new SqlCommand(sql, _connection, transaction);
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            command.Transaction = transaction;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(map(reader));
            }

            return result;
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();

            _connection.Dispose();
        }
    }
}
