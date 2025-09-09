using emirhan.kurtulus.api.core.Abstractions;
using emirhan.kurtulus.api.core.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace emirhan.kurtulus.api.core.Entities;

[CollectionName("Users")]
public class User : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string FullName { get; set; }

    [Hashed]
    public required string Password { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}