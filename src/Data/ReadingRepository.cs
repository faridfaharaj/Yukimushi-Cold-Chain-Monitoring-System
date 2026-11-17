using System.Data;
using Entities;
using Microsoft.Data.SqlClient;

namespace Data;

public class ReadingRepository : IReadingRepository
{
    private const string GET_ALL_PROCEDURE = "sp_GetReadings";
    private const string GET_BY_ID_PROCEDURE = "sp_GetReadingById";
    private const string CREATE_PROCEDURE = "sp_CreateReading";
    private const string DELETE_PROCEDURE = "sp_DeleteReading";

    private const byte VALUE_PRECISION = 10;
    private const byte VALUE_SCALE = 2;

    private readonly DbConnectionFactory _connectionFactory;

    public ReadingRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Reading>> getAllAsync(CancellationToken cancellationToken)
    {
        var readings = new List<Reading>();

        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(GET_ALL_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            readings.Add(mapReading(reader));
        }

        return readings;
    }

    public async Task<Reading?> getByIdAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(GET_BY_ID_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return mapReading(reader);
        }

        return null;
    }

    public async Task<int> createAsync(Reading reading, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        addReadingParameters(command, reading);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    public async Task<bool> deleteAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(DELETE_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

        var result = await command.ExecuteScalarAsync(cancellationToken);
        int affectedRows = Convert.ToInt32(result);
        return affectedRows > 0;
    }

    private static void addReadingParameters(SqlCommand command, Reading reading)
    {
        command.Parameters.Add(
            new SqlParameter("@Value", SqlDbType.Decimal)
            {
                Precision = VALUE_PRECISION,
                Scale = VALUE_SCALE,
                Value = reading.value,
            }
        );
    }

    private static Reading mapReading(SqlDataReader reader)
    {
        int descriptionOrdinal = reader.GetOrdinal("Description");

        return new Reading
        {
            id = reader.GetInt32(reader.GetOrdinal("Id")),
            timeStamp = reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
            value = reader.GetDecimal(reader.GetOrdinal("Value")),
        };
    }
}
