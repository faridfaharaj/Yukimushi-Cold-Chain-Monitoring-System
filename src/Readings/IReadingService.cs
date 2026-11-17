using Entities;

namespace Readings;

public interface IReadingService
{
    Task<IEnumerable<Reading>> getAllAsync(CancellationToken cancellationToken);
    Task<Reading?> getByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> createAsync(Reading reading, CancellationToken cancellationToken);
    Task<bool> deleteAsync(int id, CancellationToken cancellationToken);
}
