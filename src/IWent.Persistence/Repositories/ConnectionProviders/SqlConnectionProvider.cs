using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWent.Persistence.Repositories.ConnectionProviders;

public class SqlConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public SqlConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection InitializeConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
