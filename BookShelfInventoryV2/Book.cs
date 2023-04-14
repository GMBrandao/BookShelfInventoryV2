using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BookShelfInventoryV2
{
    internal class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Title")]
        public string? Title { get; set; }

        [BsonElement("Edition")]
        public string? Edition { get; set; }

        [BsonElement("Author")]
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

        public Book(string? title, string? edition, string? author,
            string? description, string? isbn, int numberOfPages, int currentPage)
        {
            this.Title = title;
            this.Edition = edition;
            this.Author = author;
            this.Description = description;
            this.Isbn = isbn;
            this.NumberOfPages = numberOfPages;
            this.CurrentPage = currentPage;
        }

        public Book(string? title, string? edition, string? author,
            string? description, string? isbn, int numberOfPages)
        {
            this.Title = title;
            this.Edition = edition;
            this.Author = author;
            this.Description = description;
            this.Isbn = isbn;
            this.NumberOfPages = numberOfPages;
            this.CurrentPage = 0;
        }

        public override string ToString()
        {
            return $"Título: {this.Title}\nEdição: {this.Edition}\nAutor: {this.Author}\nDescrição: " +
                $"{this.Description}\nISBN: {this.Isbn}\nNúmero de páginas: {this.NumberOfPages}\n" +
                $"Página atual: {this.CurrentPage}\n__________________\n";
        }
    }
}
