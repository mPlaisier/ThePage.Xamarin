using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IThePageService
    {
        #region Book

        Task<List<Book>> GetAllBooks();

        Task<bool> AddBook(Book book);

        Task<Book> UpdateBook(Book book);

        Task<bool> DeleteBook(Book book);

        #endregion

        #region Author

        Task<List<Author>> GetAllAuthors();

        Task<bool> AddAuthor(Author author);

        Task<Author> UpdateAuthor(Author author);

        Task<bool> DeleteAuthor(Author author);

        #endregion
    }
}
