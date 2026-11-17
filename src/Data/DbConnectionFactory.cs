using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data;

public class DbConnectionFactory
{
    private const string CONNECTION_STRING_NAME = "DefaultConnection";

    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString(CONNECTION_STRING_NAME)
            ?? throw new InvalidOperationException(
                $"Connection string '{CONNECTION_STRING_NAME}' was not found in the configuration."
            );
    }

    public SqlConnection createConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
