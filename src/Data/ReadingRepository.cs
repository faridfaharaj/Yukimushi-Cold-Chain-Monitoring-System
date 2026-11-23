using System.Data;
using System.Diagnostics;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Data;

public class ReadingRepository : IReadingRepository
{
    private const string GET_BY_DATE_RANGE_PROCEDURE = "dbo.usp_GetReadingsByDateRange";
    private const string CREATE_FLOAT_PROCEDURE = "dbo.usp_CreateFloatReading";
    private const string CREATE_GEOSPATIAL_PROCEDURE = "dbo.usp_CreateGeospatialReading";
    private const string CREATE_TRIGGER_PROCEDURE = "dbo.usp_CreateTriggerReading";
    private const string CREATE_BOOLEAN_PROCEDURE = "dbo.usp_CreateBooleanReading";
    private const string DELETE_PROCEDURE = "dbo.usp_DeleteReading";

    private readonly DbConnectionFactory _connectionFactory;
    private readonly ILogger<ReadingRepository> _logger;

    public ReadingRepository(
        DbConnectionFactory connectionFactory,
        ILogger<ReadingRepository> logger
    )
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<int> createGeospatialAsync(
        GeospatialReading reading,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Creating geospatial reading: Device={Device}, Kind={Kind}, TakenAt={TakenAt}, Lat={Latitude}, Lon={Longitude}",
            reading.device,
            reading.kind,
            reading.timeStamp,
            reading.latitude,
            reading.longitude
        );

        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_GEOSPATIAL_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(
            new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device }
        );
        command.Parameters.Add(
            new SqlParameter("@TakenAt", SqlDbType.DateTime2) { Value = reading.timeStamp }
        );
        command.Parameters.Add(
            new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind }
        );
        command.Parameters.Add(
            new SqlParameter("@Longitude", SqlDbType.Decimal)
            {
                Precision = 9,
                Scale = 6,
                Value = (decimal)reading.longitude,
            }
        );
        command.Parameters.Add(
            new SqlParameter("@Latitude", SqlDbType.Decimal)
            {
                Precision = 9,
                Scale = 6,
                Value = (decimal)reading.latitude,
            }
        );

        int id = await executeCreateAsync(
            command,
            CREATE_GEOSPATIAL_PROCEDURE,
            reading.device,
            cancellationToken
        );

        _logger.LogInformation(
            "Created geospatial reading: Id={ReadingId}, Device={Device}, Kind={Kind}, TakenAt={TakenAt:o}, Latitude={Latitude}, Longitude={Longitude}",
            id,
            reading.device,
            reading.kind,
            reading.timeStamp,
            reading.latitude,
            reading.longitude
        );

        return id;
    }

    public async Task<int> createFloatAsync(
        FloatReading reading,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Creating float reading: Device={Device}, Kind={Kind}, TakenAt={TakenAt}, Value={Value}",
            reading.device,
            reading.kind,
            reading.timeStamp,
            reading.value
        );

        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_FLOAT_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(
            new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device }
        );
        command.Parameters.Add(
            new SqlParameter("@TakenAt", SqlDbType.DateTime2) { Value = reading.timeStamp }
        );
        command.Parameters.Add(
            new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind }
        );
        command.Parameters.Add(
            new SqlParameter("@Value", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 4,
                Value = (decimal)reading.value,
            }
        );

        int id = await executeCreateAsync(
            command,
            CREATE_FLOAT_PROCEDURE,
            reading.device,
            cancellationToken
        );

        _logger.LogInformation(
            "Created float reading: Id={ReadingId}, Device={Device}, Kind={Kind}, TakenAt={TakenAt:o}, Value={Value}",
            id,
            reading.device,
            reading.kind,
            reading.timeStamp,
            reading.value
        );

        return id;
    }

    public async Task<int> createTriggerAsync(
        TriggerReading reading,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Creating trigger reading: Device={Device}, Kind={Kind}, TakenAt={TakenAt}",
            reading.device,
            reading.kind,
            reading.timeStamp
        );

        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_TRIGGER_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(
            new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device }
        );
        command.Parameters.Add(
            new SqlParameter("@TakenAt", SqlDbType.DateTime2) { Value = reading.timeStamp }
        );
        command.Parameters.Add(
            new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind }
        );

        int id = await executeCreateAsync(
            command,
            CREATE_TRIGGER_PROCEDURE,
            reading.device,
            cancellationToken
        );

        _logger.LogInformation(
            "Created trigger reading: Id={ReadingId}, Device={Device}, Kind={Kind}, TakenAt={TakenAt:o}",
            id,
            reading.device,
            reading.kind,
            reading.timeStamp
        );

        return id;
    }

    public async Task<int> createBooleanAsync(
        BooleanReading reading,
        CancellationToken cancellationToken
    )
    {
        _logger.LogDebug(
            "Creating boolean reading: Device={Device}, Kind={Kind}, TakenAt={TakenAt}, State={State}",
            reading.device,
            reading.kind,
            reading.timeStamp,
            reading.state
        );

        await using var connection = _connectionFactory.createConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(CREATE_BOOLEAN_PROCEDURE, connection)
        {
            CommandType = CommandType.StoredProcedure,
        };

        command.Parameters.Add(
            new SqlParameter("@Device", SqlDbType.NVarChar) { Value = reading.device }
        );
        command.Parameters.Add(
            new SqlParameter("@TakenAt", SqlDbType.DateTime2) { Value = reading.timeStamp }
        );
        command.Parameters.Add(
            new SqlParameter("@Kind", SqlDbType.NVarChar) { Value = reading.kind }
        );
        command.Parameters.Add(new SqlParameter("@State", SqlDbType.Bit) { Value = reading.state });

        int id = await executeCreateAsync(
            command,
            CREATE_BOOLEAN_PROCEDURE,
            reading.device,
            cancellationToken
        );

        _logger.LogInformation(
            "Created boolean reading: Id={ReadingId}, Device={Device}, Kind={Kind}, TakenAt={TakenAt:o}, State={State}",
            id,
            reading.device,
            reading.kind,
            reading.timeStamp,
            reading.state
        );

        return id;
    }

    public async Task<bool> deleteAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting reading {ReadingId}", id);
        var stopwatch = Stopwatch.StartNew();

        try
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
            bool deleted = affectedRows > 0;

            if (deleted)
            {
                _logger.LogInformation(
                    "Deleted reading {ReadingId} in {ElapsedMs} ms",
                    id,
                    stopwatch.ElapsedMilliseconds
                );
            }
            else
            {
                _logger.LogWarning(
                    "Delete of reading {ReadingId} affected 0 rows (not found) after {ElapsedMs} ms",
                    id,
                    stopwatch.ElapsedMilliseconds
                );
            }

            return deleted;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Delete of reading {ReadingId} was cancelled", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to delete reading {ReadingId} after {ElapsedMs} ms",
                id,
                stopwatch.ElapsedMilliseconds
            );
            throw;
        }
    }

    public async Task<IEnumerable<Reading>> getByDateRangeAsync(
        DateTime from,
        DateTime to,
        string? device,
        string? group,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "Querying readings: From={From}, To={To}, Device={Device}, Group={Group}",
            from,
            to,
            device ?? "(any)",
            group ?? "(any)"
        );
        var stopwatch = Stopwatch.StartNew();
        int rowIndex = 0;

        try
        {
            var readings = new List<Reading>();

            await using var connection = _connectionFactory.createConnection();
            await connection.OpenAsync(cancellationToken);
            _logger.LogDebug("Connection opened in {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);

            await using var command = new SqlCommand(GET_BY_DATE_RANGE_PROCEDURE, connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.Add(new SqlParameter("@From", SqlDbType.DateTime2) { Value = from });
            command.Parameters.Add(new SqlParameter("@To", SqlDbType.DateTime2) { Value = to });
            command.Parameters.Add(
                new SqlParameter("@Device", SqlDbType.NVarChar)
                {
                    Value = string.IsNullOrWhiteSpace(device) ? DBNull.Value : device,
                }
            );
            command.Parameters.Add(
                new SqlParameter("@Group", SqlDbType.NVarChar)
                {
                    Value = string.IsNullOrWhiteSpace(group) ? DBNull.Value : group,
                }
            );

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            _logger.LogDebug(
                "Procedure {Procedure} started returning rows after {ElapsedMs} ms",
                GET_BY_DATE_RANGE_PROCEDURE,
                stopwatch.ElapsedMilliseconds
            );

            while (await reader.ReadAsync(cancellationToken))
            {
                rowIndex++;
                readings.Add(mapReading(reader));
            }

            _logger.LogInformation(
                "Query returned {Count} readings in {ElapsedMs} ms",
                readings.Count,
                stopwatch.ElapsedMilliseconds
            );

            if (readings.Count == 0)
            {
                _logger.LogDebug("No readings matched the filters");
            }

            return readings;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning(
                "Readings query was cancelled after {ElapsedMs} ms",
                stopwatch.ElapsedMilliseconds
            );
            throw;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "SQL error {SqlErrorNumber} in {Procedure} after {ElapsedMs} ms",
                ex.Number,
                GET_BY_DATE_RANGE_PROCEDURE,
                stopwatch.ElapsedMilliseconds
            );
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to read or map row {RowIndex} from {Procedure} after {ElapsedMs} ms",
                rowIndex,
                GET_BY_DATE_RANGE_PROCEDURE,
                stopwatch.ElapsedMilliseconds
            );
            throw;
        }
    }

    private async Task<int> executeCreateAsync(
        SqlCommand command,
        string procedure,
        string device,
        CancellationToken cancellationToken
    )
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await command.ExecuteScalarAsync(cancellationToken);
            int id = Convert.ToInt32(result);

            _logger.LogInformation(
                "{Procedure} created reading {ReadingId} for device {Device} in {ElapsedMs} ms",
                procedure,
                id,
                device,
                stopwatch.ElapsedMilliseconds
            );
            return id;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("{Procedure} was cancelled for device {Device}", procedure, device);
            throw;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "SQL error {SqlErrorNumber} in {Procedure} for device {Device} after {ElapsedMs} ms",
                ex.Number,
                procedure,
                device,
                stopwatch.ElapsedMilliseconds
            );
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error in {Procedure} for device {Device} (result could not be read as an int?)",
                procedure,
                device
            );
            throw;
        }
    }

    private Reading mapReading(SqlDataReader reader)
    {
        Reading reading;
        string type;

        if (hasValue(reader, "Value"))
        {
            type = "float";
            reading = mapFloatReading(reader);
        }
        else if (hasValue(reader, "State"))
        {
            type = "boolean";
            reading = mapBooleanReading(reader);
        }
        else if (hasValue(reader, "Longitude"))
        {
            type = "geospatial";
            reading = mapGeospatialReading(reader);
        }
        else
        {
            type = "trigger";
            reading = mapTriggerReading(reader);
        }

        reading.device = reader.GetString(reader.GetOrdinal("Device"));

        _logger.LogTrace("Mapped row as {ReadingType} reading {ReadingId}", type, reading.id);
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
}
