using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public class ThePageService : IThePageService
    {
        #region Public(Books)

        public async Task<List<Book>> GetAllBooks()
        {
            return await BookManager.FetchBooks(CancellationToken.None);
        }

        public async Task<bool> AddBook(Book book)
        {
            var result = await BookManager.AddBook(book, CancellationToken.None);

            return result != null;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            return await BookManager.UpdateBook(book, CancellationToken.None);
        }

        public async Task<bool> DeleteBook(Book content)
        {
            return await BookManager.DeleteBook(content, CancellationToken.None);
        }

        #endregion

        #region Authors

        public async Task<List<Author>> GetAllAuthors()
        {
            return await AuthorManager.FetchAuthors(CancellationToken.None);
        }

        public async Task<bool> AddAuthor(Author author)
        {
            var result = await AuthorManager.AddAuthor(author, CancellationToken.None);

            return result != null;
        }

        public async Task<Author> UpdateAuthor(Author author)
        {
            return await AuthorManager.UpdateAuthor(author, CancellationToken.None);
        }

        public async Task<bool> DeleteAuthor(Author author)
        {
            return await AuthorManager.DeleteAuthor(author, CancellationToken.None);
        }

        #endregion
    }
}
