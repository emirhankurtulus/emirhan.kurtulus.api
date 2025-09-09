using System.Reflection;
using emirhan.kurtulus.api.core.Abstractions;
using emirhan.kurtulus.api.core.Attributes;
using MongoDB.Driver;

namespace emirhan.kurtulus.api.infrastructure.Mongo;

public class MongoInitializer(IMongoDatabase database)
{
    private readonly IMongoDatabase _database = database;

    public async Task InitializeAsync()
    {
        var existingCollections = await _database.ListCollectionNames().ToListAsync();

        var entityTypes = Assembly
            .GetAssembly(typeof(IEntity))!
            .GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

        foreach (var type in entityTypes)
        {
            var collectionName = type.GetCustomAttribute<CollectionNameAttribute>()?.Name
                                 ?? type.Name;

            if (!existingCollections.Contains(collectionName))
            {
                await _database.CreateCollectionAsync(collectionName);
            }
        }
    }
}
