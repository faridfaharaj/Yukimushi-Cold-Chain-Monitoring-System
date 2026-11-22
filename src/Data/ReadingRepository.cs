using System.Data;
using Entities;
using Microsoft.Data.SqlClient;

namespace Data;

public class ReadingRepository : IReadingRepository
{
    private const string GET_ALL_PROCEDURE = "sp_GetReadings";
    private const string GET_BY_ID_PROCEDURE = "sp_GetReadingById";

    private const string GET_BY_DATE_RANGE_PROCEDURE = "sp_GetReadingByDateRange";

    private const string CREATE_FLOAT_PROCEDURE = "sp_CreateReading";

    private const string CREATE_GEOSPATIAL_PROCEDURE = "sp_CreateReading";

    private const string CREATE_TRIGGER_PROCEDURE = "sp_CreateReading";

    private const string CREATE_BOOLEAN_PROCEDURE = "sp_CreateReading";

    private const string DELETE_PROCEDURE = "sp_DeleteReading";

    private const byte VALUE_PRECISION = 10;
    private const byte VALUE_SCALE = 2;

    private readonly DbConnectionFactory _connectionFactory;

    public ReadingRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> createGeospatialAsync(
        GeospatialReading reading,
        CancellationToken cancellationToken
    )
    {
        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_GEOSPATIAL_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device });
        command.Parameters.Add(new SqlParameter("@TakenAt", SqlDbType.NVarChar) { Value = reading.timeStamp });
        command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind });
        
        command.Parameters.Add(new SqlParameter("@Longitude", SqlDbType.NVarChar) { Value = reading.longitude });
        command.Parameters.Add(new SqlParameter("@Latitude", SqlDbType.NVarChar) { Value = reading.latitude });

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    public async Task<int> createFloatAsync(
        FloatReading reading,
        CancellationToken cancellationToken
    )
    {
        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_FLOAT_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device });
        command.Parameters.Add(new SqlParameter("@TakenAt", SqlDbType.NVarChar) { Value = reading.timeStamp });
        command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind });
        
        command.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar) { Value = reading.value });

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    public async Task<int> createTriggerAsync(
        TriggerReading reading,
        CancellationToken cancellationToken
    )
    {
        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_TRIGGER_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device });
        command.Parameters.Add(new SqlParameter("@TakenAt", SqlDbType.NVarChar) { Value = reading.timeStamp });
        command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind });

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    public async Task<int> createBooleanAsync(
        BooleanReading reading,
        CancellationToken cancellationToken
    )
    {
        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_BOOLEAN_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device });
        command.Parameters.Add(new SqlParameter("@TakenAt", SqlDbType.NVarChar) { Value = reading.timeStamp });
        command.Parameters.Add(new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind });
        
        command.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar) { Value = reading.state });

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

    // TODO: Finish this
    /*
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
    */

    private static Reading mapReading(SqlDataReader reader)
    {
        Reading reading;

        if (hasValue(reader, "Value"))
        {
            reading = mapFloatReading(reader);
        }
        else if (hasValue(reader, "State"))
        {
            reading = mapBooleanReading(reader);
        }
        else if (hasValue(reader, "Longitude"))
        {
            reading = mapGeospatialReading(reader);
        }
        else
        {
            reading = mapTriggerReading(reader);
        }

        reading.device = reader.GetString(reader.GetOrdinal("Device"));

        return reading;
    }

    private static bool hasValue(SqlDataReader reader, string column)
    {
        return !reader.IsDBNull(reader.GetOrdinal(column));
    }

    private static Reading mapBooleanReading(SqlDataReader reader)
    {
        int descriptionOrdinal = reader.GetOrdinal("Description");

        return new BooleanReading
        {
            id = reader.GetInt32(reader.GetOrdinal("Id")),
            device = reader.GetString(reader.GetOrdinal("Device")),
            kind = reader.GetString(reader.GetOrdinal("Kind")),
            timeStamp = reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
            state = reader.GetBoolean(reader.GetOrdinal("State")),
        };
    }

    private static Reading mapTriggerReading(SqlDataReader reader)
    {
        int descriptionOrdinal = reader.GetOrdinal("Description");

        return new TriggerReading
        {
            id = reader.GetInt32(reader.GetOrdinal("Id")),
            device = reader.GetString(reader.GetOrdinal("Device")),
            kind = reader.GetString(reader.GetOrdinal("Kind")),
            timeStamp = reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
        };
    }

    private static Reading mapFloatReading(SqlDataReader reader)
    {
        int descriptionOrdinal = reader.GetOrdinal("Description");

        return new FloatReading
        {
            id = reader.GetInt32(reader.GetOrdinal("Id")),
            device = reader.GetString(reader.GetOrdinal("Device")),
            kind = reader.GetString(reader.GetOrdinal("Kind")),
            timeStamp = reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
            value = reader.GetFloat(reader.GetOrdinal("Value")),
        };
    }

    private static Reading mapGeospatialReading(SqlDataReader reader)
    {
        int descriptionOrdinal = reader.GetOrdinal("Description");

        return new GeospatialReading
        {
            id = reader.GetInt32(reader.GetOrdinal("Id")),
            device = reader.GetString(reader.GetOrdinal("Device")),
            kind = reader.GetString(reader.GetOrdinal("Kind")),
            timeStamp = reader.GetDateTime(reader.GetOrdinal("TimeStamp")),
            longitude = reader.GetDouble(reader.GetOrdinal("Longitude")),
            latitude = reader.GetFloat(reader.GetOrdinal("Latitude")),
        };
    }

    public async Task<IEnumerable<Reading>> getByDateRangeAsync(
        string device,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        var readings = new List<Reading>();

        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(GET_BY_DATE_RANGE_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(new SqlParameter("@Device", SqlDbType.NVarChar) { Value = device });
        command.Parameters.Add(new SqlParameter("@From", SqlDbType.DateTime2) { Value = from });
        command.Parameters.Add(new SqlParameter("@To", SqlDbType.DateTime2) { Value = to });

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            readings.Add(mapReading(reader));
        }

        return readings;
    }
}
