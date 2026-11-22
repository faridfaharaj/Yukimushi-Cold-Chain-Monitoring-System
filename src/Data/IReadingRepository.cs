using Entities;

namespace Data;

public interface IReadingRepository
{
    Task<bool> deleteAsync(int id, CancellationToken cancellationToken);

    Task<int> createGeospatialAsync(GeospatialReading reading, CancellationToken cancellationToken);

    Task<int> createFloatAsync(FloatReading reading, CancellationToken cancellationToken);

    Task<int> createTriggerAsync(TriggerReading reading, CancellationToken cancellationToken);

    Task<int> createBooleanAsync(BooleanReading reading, CancellationToken cancellationToken);

    Task<IEnumerable<Reading>> getByDateRangeAsync(
        string device,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    );
}
