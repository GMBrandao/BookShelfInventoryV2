using BookShelfInventoryV2;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

internal class Program
{
    private static void Main(string[] args)
    {

        MongoClient mongo = new MongoClient("mongodb://localhost:27017");

        var database = mongo.GetDatabase("DB_BookShelf");
        var bookCollection = database.GetCollection<BsonDocument>("Bookshelf");
        var lentCollection = database.GetCollection<BsonDocument>("BookLent");

        do
        {
            BsonDocument bookBson;

            switch (Menu())
            {
                case "1":
                    CreateBook(bookCollection);
                    Console.WriteLine("Livro adicionado com sucesso");
                    break;

                case "3":
                    bookBson = FindBookByEqualTitle(bookCollection, "para apagar");
                    if (DeleteBook(bookCollection, bookBson))
                        Console.WriteLine("Livro deletado com sucesso");
                    else
                        Console.WriteLine("Livro não encontrado");
                    break;

                case "4":
                    if(ReadBook(bookCollection))
                        Console.WriteLine("Página atual atualizada com sucesso");
                    else
                        Console.WriteLine("Livro não encontrado");
                    break;

                case "5":
                    ShowBookShelf(bookCollection);
                    break;

                case "6":
                    bookBson = FindBookByEqualTitle(bookCollection, "para emprestar");
                    if (LentBook(bookCollection, lentCollection, bookBson))
                        Console.WriteLine("Livro emprestado com sucesso");
                    else
                        Console.WriteLine("Livro não encontrado");
                    break;

                case "7":
                    bookBson = FindBookByEqualTitle(lentCollection, "para retornar");
                    if (LentBook(lentCollection, bookCollection, bookBson))
                        Console.WriteLine("Livro retornado com sucesso");
                    else
                        Console.WriteLine("Livro não encontrado");
                    break;

                case "8":
                    ShowBookShelf(lentCollection);
                    break;

                case "9":
                    Console.WriteLine("Encerrando...");
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }

            Console.Write("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
            Console.Clear();

        } while (true);
    }

    private static bool LentBook(IMongoCollection<BsonDocument> firstCollection, IMongoCollection<BsonDocument> secondCollection, BsonDocument bookBson)
    {
        if (bookBson != null)
        {
            var book = BsonSerializer.Deserialize<Book>(bookBson);

            firstCollection.DeleteOne(bookBson);
            var bk = new BsonDocument
            {
            { "Title", book.Title },
            { "Edition", book.Edition},
            { "Author", book.Author},
            { "Description", book.Description},
            { "Isbn", book.Isbn},
            { "NumberOfPages", book.NumberOfPages},
            { "CurrentPage", book.CurrentPage}
            };

            secondCollection.InsertOne(bk);
            return true;

        }
        return false;
    }

    private static bool ReadBook(IMongoCollection<BsonDocument> collection)
    {
        int page;
        Console.WriteLine("Informe o nome do livro que deseja ler: ");
        string i = Console.ReadLine();

        var filter = Builders<BsonDocument>.Filter.Eq("Title", i);
        var bookBson = collection.Find(filter).FirstOrDefault();

        if (bookBson != null)
        {
            var book = BsonSerializer.Deserialize<Book>(bookBson);

            do
            {
                page = int.Parse(VerifyString('o', "número da página que parou de ler"));
                book.CurrentPage = page;
            } while (page > book.NumberOfPages);

            var update = Builders<BsonDocument>.Update.Set("CurrentPage", book.CurrentPage);

            collection.UpdateOne(filter, update);

            return true;
        }
        return false;
    }

    private static BsonDocument FindBookByEqualTitle(IMongoCollection<BsonDocument> collection, string s)
    {
        Console.WriteLine($"Informe o nome do livro {s}: ");
        var i = Console.ReadLine();

        var filter = Builders<BsonDocument>.Filter.Eq("Title", i);
        var book = collection.Find(filter).FirstOrDefault();

        return book;
    }

    private static bool DeleteBook(IMongoCollection<BsonDocument> collection, BsonDocument book)
    {
        if (book != null)
        {
            collection.DeleteOne(book);
            return true;
        }
        return false;
    }

    private static void ShowBookShelf(IMongoCollection<BsonDocument> bookshelf)
    {
        var documents = bookshelf.Find(new BsonDocument()).ToList();
        List<Book> booklist = new();
        //documents.ForEach(x => Console.WriteLine(x.ToString()));
        foreach (var item in documents)
        {
            var aux = BsonSerializer.Deserialize<Book>(item);
            booklist.Add(aux);
        }

        booklist.ForEach(x => Console.WriteLine(x.ToString()));

    }

    private static void CreateBook(IMongoCollection<BsonDocument> collection)
    {
        bool p;
        string title = VerifyString('o', "Título");
        string edition = VerifyString('a', "Edição");
        string author = VerifyString('o', "Autor");
        string description = VerifyString('a', "Descrição");
        string isbn = VerifyIsbn();
        int pages;
        do
        {
            p = int.TryParse(VerifyString('o', "número de páginas"), out pages);
            if (p == false)
            {
                Console.WriteLine("Páginas devem ser um número inteiro");
            }
        } while (!p);

        Book book = new(title, edition, author, description, isbn, pages);

        InsertBook(collection, book);
    }

    private static void InsertBook(IMongoCollection<BsonDocument> collection, Book book)
    {
        var bk = new BsonDocument
        {
            { "Title", book.Title },
            { "Edition", book.Edition},
            { "Author", book.Author},
            { "Description", book.Description},
            { "Isbn", book.Isbn},
            { "NumberOfPages", book.NumberOfPages},
            { "CurrentPage", book.CurrentPage}
        };

        collection.InsertOne(bk);
    }

    private static string VerifyIsbn()
    {
        string isbn;
        bool length;
        int aux = 0;

        do
        {
            length = false;

            if (aux > 0)
            {
                Console.WriteLine("O ISBN deve ter 10 ou 13 dígitos.");
            }

            isbn = VerifyString('o', "ISBN");
            if (isbn.Length == 10 || isbn.Length == 13)
                length = true;

            aux++;
        } while (!length);
        return isbn;
    }

    private static string VerifyString(char article, string variable)
    {
        string verified;
        bool aux = true;

        do
        {
            Console.Write($"Informe {article} {variable}: ");
            verified = Console.ReadLine();
            aux = string.IsNullOrEmpty(verified);
            if (aux)
                Console.WriteLine($"{variable} inválid{article}");
        } while (aux);

        return verified;
    }

    static string Menu()
    {
        Console.WriteLine("\t>>>>> MENU <<<<<\n1 - Inserir Livro na Estante\n3 - Deletar Livro" +
            "\n4 - Inserir sessão de leitura de um Livro\n5 - Mostrar Estante\n6 - Emprestar Livro da Estante\n" +
            "7 - Retornar Livro a Estante\n8 - Mostrar Livros emprestados\n9 - Sair\n\nEscolha uma opção: ");
        return Console.ReadLine();
    }
}