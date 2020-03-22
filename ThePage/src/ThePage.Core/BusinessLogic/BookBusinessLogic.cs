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
            return booksApi.Select(x => new BookCell(x.Id, x.Title, authorsApi.FirstOrDefault(a => a.Id == x.Author))).ToList();
        }

        public static Book BookCellToBook(BookCell bookCell)
        {
            return new Book(bookCell.Id, bookCell.Title, bookCell.Author.Id);
        }

        #endregion
    }
}
