using emirhan.kurtulus.api.core.Abstractions;
using System.Linq.Expressions;

namespace emirhan.kurtulus.api.core.Repositories;


public interface IMongoRepository<T> where T : IEntity
{
    Task AddAsync(T document, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, T document, CancellationToken ct = default);
    Task<T?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
    Task<bool> Delete(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
}