using emirhan.kurtulus.api.application.Services;
using emirhan.kurtulus.api.core.Abstractions;
using emirhan.kurtulus.api.core.Attributes;
using emirhan.kurtulus.api.core.Repositories;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace emirhan.kurtulus.api.infrastructure.Mongo;

public class MongoRepository<T>(
    IMongoDatabase mongoDataBase,
    IPasswordService passwordService) : IMongoRepository<T> where T : IEntity
{
    private readonly IMongoDatabase _mongoDataBase = mongoDataBase;
    private readonly IPasswordService _passwordService = passwordService;
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _hashedPropsCache = new();

    public async Task AddAsync(T document, CancellationToken cancellationToken = default)
    {
        GuardNotNull(document);
        HashAnnotatedStringProps(document);
        await GetCollection().InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAsync(Guid id, T document, CancellationToken cancellationToken = default)
    {
        GuardNotNull(document);
        HashAnnotatedStringProps(document);

        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var result = await GetCollection().ReplaceOneAsync(filter, document, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        return await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var col = GetCollection();
        return predicate is null
            ? await col.Find(FilterDefinition<T>.Empty).ToListAsync(cancellationToken)
            : await col.Find(predicate).ToListAsync(cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, id);
        return await GetCollection().Find(filter).Limit(1).AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var col = GetCollection();
        return predicate is null
            ? await col.Find(FilterDefinition<T>.Empty).Limit(1).AnyAsync(cancellationToken)
            : await col.Find(predicate).Limit(1).AnyAsync(cancellationToken);
    }

    private IMongoCollection<T> GetCollection()
    {
        var type = typeof(T);
        var nameAttr = type.GetCustomAttribute<CollectionNameAttribute>();
        var collectionName = nameAttr?.Name ?? type.Name;
        return _mongoDataBase.GetCollection<T>(collectionName);
    }

    private static void GuardNotNull(object? doc)
    {
        ArgumentNullException.ThrowIfNull(doc);
    }

    private void HashAnnotatedStringProps(T document)
    {
        var type = typeof(T);
        var props = _hashedPropsCache.GetOrAdd(type, t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
             .Where(p => p.GetCustomAttribute(typeof(HashedAttribute)) is not null &&
                         p.CanRead && p.CanWrite &&
                         p.PropertyType == typeof(string))
             .ToArray()
        );

        foreach (var p in props)
        {
            var current = p.GetValue(document) as string;
            if (!string.IsNullOrWhiteSpace(current))
            {
                var hashed = _passwordService.HashPassword(current);
                p.SetValue(document, hashed);
            }
        }
    }
}