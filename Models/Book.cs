using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Title")]
        public string? Title { get; set; }
        
        [BsonElement]
        public string? Edition { get; set; }

        [BsonElement]
        public string? Author { get; set; }

        [BsonElement("Description")]
        public string? Description { get; set; }

        [BsonElement("Isbn")]
        public string? Isbn { get; set; }

        [BsonElement("NumberOfPages")]
        [BsonRepresentation(BsonType.Int32)]
        public int NumberOfPages { get; set; }

        [BsonElement("CurrentPage")]
        [BsonRepresentation(BsonType.Int32)]
        public int CurrentPage { get; set; }
    }
}