using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UpworkProject.Models
{
    [BsonIgnoreExtraElements]
    public class Image
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string SessionId { get; set; } = default!;
        public string ImageName { get; set; } = default!;
    }
}
