using Entities;

namespace Readings;

public interface IReadingService
{
    Task<int> createGeospatialAsync(GeospatialReading reading, CancellationToken cancellationToken);
    Task<int> createFloatAsync(FloatReading reading, CancellationToken cancellationToken);

    Task<int> createTriggerAsync(TriggerReading reading, CancellationToken cancellationToken);

    Task<int> createBooleanAsync(BooleanReading reading, CancellationToken cancellationToken);

    Task<bool> deleteAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Reading>> getByDateRangeAsync(
        DateTime from,
        DateTime to,
        string? device,
        string? group,
        CancellationToken cancellationToken
    );
}
