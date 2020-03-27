using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public class ThePageService : IThePageService
    {
        #region Public(Books)

        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> result = null;
            try
            {
                result = await BookManager.FetchBooks();
            }
            catch (Exception ex)
            {
                HandleException(ex);

            }
            return result;
        }

        public async Task<Book> GetBook(string id)
        {
            Book result = null;
            try
            {
                result = await BookManager.FetchBook(id);
            }
            catch (Exception ex)
            {
                HandleException(ex);

            }
            return result;
        }

        public async Task<bool> AddBook(Book book)
        {
            var result = await BookManager.AddBook(book);

            return result != null;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            return await BookManager.UpdateBook(book);
        }

        public async Task<bool> DeleteBook(Book content)
        {
            return await BookManager.DeleteBook(content);
        }

        #endregion

        #region Authors

        public async Task<List<Author>> GetAllAuthors()
        {
            return await AuthorManager.FetchAuthors();
        }

        public async Task<bool> AddAuthor(Author author)
        {
            var result = await AuthorManager.AddAuthor(author);

            return result != null;
        }

        public async Task<Author> UpdateAuthor(Author author)
        {
            return await AuthorManager.UpdateAuthor(author);
        }

        public async Task<bool> DeleteAuthor(Author author)
        {
            return await AuthorManager.DeleteAuthor(author);
        }

        #endregion

        #region Private

        //TODO perhaps move to a general Utils class/file
        void HandleException(Exception ex)
        {
            var message = "";
            if (ex is ApiException apiEx)
            {
                if (apiEx.ApiError.Code == EApiErrorCode.BookNotFound)
                {
                    message = "Book not found.";
                }

            }
            //TODO show message or return error?
        }

        #endregion

    }
}
