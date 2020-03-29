using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class BookManager
    {
        #region Properties

        static readonly IBookAPI _bookAPI = RestService.For<IBookAPI>(Secrets.ThePageAPI_URL);

        #endregion

        #region FETCH

        public static async Task<List<Book>> FetchBooks()
        {
            return await _bookAPI.GetBooks();
        }

        public static async Task<Book> FetchBook(string id)
        {
            return await _bookAPI.GetBook(id);
        }

        #endregion

        #region ADD

        public static async Task<Book> AddBook(Book book)
        {
            return await _bookAPI.AddBook(book);
        }

        #endregion

        #region PATCH

        public static async Task<Book> UpdateBook(Book book)
        {
            return await _bookAPI.UpdateBook(book);
        }

        #endregion

        #region DELETE

        public static async Task<bool> DeleteBook(Book book)
        {
            await _bookAPI.DeleteBook(book);
            return true;
        }

        #endregion
    }
}