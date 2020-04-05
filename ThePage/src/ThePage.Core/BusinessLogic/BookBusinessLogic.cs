using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class BookBusinessLogic
    {
        #region Public

        public static List<BookCell> BooksToBookCells(List<Book> booksApi, List<Author> authorsApi, List<Genre> genresApi)
        {
            return booksApi.Select(x => BookToBookCell(x, authorsApi, genresApi)).ToList();
        }

        public static BookCell BookToBookCell(Book book, List<Author> authorsApi, List<Genre> genresApi)
        {
            if (book.Genres == null)
                book.Genres = new List<string>();

            var genres = genresApi.Where(g => book.Genres.Contains(g.Id)).ToList();
            var author = authorsApi.FirstOrDefault(a => a.Id == book.Author);

            return new BookCell(book, author, genres);
        }

        #endregion
    }
}
