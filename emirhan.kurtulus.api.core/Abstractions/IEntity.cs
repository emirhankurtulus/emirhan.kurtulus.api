using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace emirhan.kurtulus.api.core.Abstractions;

public interface IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }   
}