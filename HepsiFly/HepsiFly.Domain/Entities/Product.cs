using HepsiFly.Domain.Base;
using HepsiFly.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HepsiFly.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Currency Currency { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId {get; set;}
    [BsonIgnore]
    public Category? Category {get; set;}
}