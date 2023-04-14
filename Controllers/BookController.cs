using Models;

namespace Controllers
{
    public class BookController
    {

        public Book InsertBook(string t, string e, string a, string d, string i, int nof, int cp)
        {
            Book book = new();

            book.Title = t;
            book.Edition = e;
            book.Author = a;
            book.Description = d;
            book.Isbn = i;
            book.NumberOfPages = nof;
            book.CurrentPage = cp;

            return book;
        }

    }
}