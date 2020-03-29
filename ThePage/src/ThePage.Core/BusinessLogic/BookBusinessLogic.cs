using System;
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
            var genres = genresApi.Where(g => book.Genres.Contains(g.Id)).ToList();
            var author = authorsApi.FirstOrDefault(a => a.Id == book.Author);

            return new BookCell(book.Id, book.Title, author, genres);
        }

        public static Book BookCellToBook(BookCell bookCell)
        {
            var genres = bookCell.Genres?.Select(g => g.Id).ToList();
            return new Book(bookCell.Id, bookCell.Title, bookCell.Author?.Id, genres);
        }

        #endregion
    }
}
