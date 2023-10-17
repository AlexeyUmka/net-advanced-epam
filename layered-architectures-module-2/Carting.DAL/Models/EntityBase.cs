using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Carting.DAL.Models;

public class EntityBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}