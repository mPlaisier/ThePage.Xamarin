using System;
using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class BookBusinessLogic
    {
        #region Public

        public static List<BookCell> BooksToBookCells(List<Book> booksApi, List<Author> authorsApi)
        {
            return booksApi.Select(x => BookToBookCell(x, authorsApi)).ToList();
        }

        public static BookCell BookToBookCell(Book book, List<Author> authorsApi)
        {
            return new BookCell(book.Id, book.Title, authorsApi.FirstOrDefault(a => a.Id == book.Author));
        }

        public static Book BookCellToBook(BookCell bookCell)
        {
            return new Book(bookCell.Id, bookCell.Title, bookCell.Author.Id);
        }

        #endregion
    }
}
