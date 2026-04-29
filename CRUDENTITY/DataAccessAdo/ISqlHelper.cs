using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.DataAccessAdo
{
    public interface ISqlHelper
    {
        Task<List<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> map, SqlParameter[]? parameters = null, SqlTransaction? transaction = null);
        Task<int> ExecuteAsync(string sql, SqlParameter[]? parameters = null, SqlTransaction? transaction = null);
        //SqlConnection CreateOpenConnection();

        SqlConnection Connection { get; }
        SqlTransaction? Transaction { get; }
    }
}
