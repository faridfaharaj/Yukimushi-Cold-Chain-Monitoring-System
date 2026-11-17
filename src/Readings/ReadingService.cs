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

    public async Task<IEnumerable<Reading>> getAllAsync(CancellationToken cancellationToken)
    {
        return await _repository.getAllAsync(cancellationToken);
    }

    public async Task<Reading?> getByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _repository.getByIdAsync(id, cancellationToken);
    }

    public async Task<int> createAsync(Reading reading, CancellationToken cancellationToken)
    {
        return await _repository.createAsync(reading, cancellationToken);
    }

    public async Task<bool> deleteAsync(int id, CancellationToken cancellationToken)
    {
        return await _repository.deleteAsync(id, cancellationToken);
    }
}
