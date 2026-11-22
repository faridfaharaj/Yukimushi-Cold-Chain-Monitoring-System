using Data;
using Entities;

namespace Readings;

public class ReadingService : IReadingService
{
    private readonly IReadingRepository _repository;

    public ReadingService(IReadingRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> deleteAsync(int id, CancellationToken cancellationToken)
    {
        return await _repository.deleteAsync(id, cancellationToken);
    }

    public async Task<int> createGeospatialAsync(
        GeospatialReading reading,
        CancellationToken cancellationToken
    )
    {
        return await _repository.createGeospatialAsync(reading, cancellationToken);
    }

    public async Task<int> getRangeAsync(
        GeospatialReading reading,
        CancellationToken cancellationToken
    )
    {
        return await _repository.createGeospatialAsync(reading, cancellationToken);
    }

    public async Task<int> createFloatAsync(
        FloatReading reading,
        CancellationToken cancellationToken
    )
    {
        return await _repository.createFloatAsync(reading, cancellationToken);
    }

    public async Task<int> createTriggerAsync(
        TriggerReading reading,
        CancellationToken cancellationToken
    )
    {
        return await _repository.createTriggerAsync(reading, cancellationToken);
    }

    public async Task<int> createBooleanAsync(
        BooleanReading reading,
        CancellationToken cancellationToken
    )
    {
        return await _repository.createBooleanAsync(reading, cancellationToken);
    }

    public async Task<IEnumerable<Reading>> getByDateRangeAsync(
        string device,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken
    )
    {
        return await _repository.getByDateRangeAsync(device, from, to, cancellationToken);
    }
}
