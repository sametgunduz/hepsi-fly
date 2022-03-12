using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HepsiFly.Domain.Base;

public abstract class BaseEntity : IBaseEntity<string>
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    [BsonElement(Order = 0)]
    public string Id { get; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement(Order = 101)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
}