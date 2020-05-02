using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class BookManager
    {
        #region CachingKeys

        const string FetchBooksKey = "FetchBooksKey";
        const string GetSingleBookKey = "GetBookKey";

        #endregion

        #region Properties

        static readonly IBookAPI _bookAPI = RestService.For<IBookAPI>(Secrets.ThePageAPI_URL);

        #endregion

        #region FETCH

        public static async Task<List<Book>> FetchBooks(bool forceRefresh = false)
        {
            List<Book> result = null;
            if (!forceRefresh && !Barrel.Current.Exists(FetchBooksKey) && !Barrel.Current.IsExpired(FetchBooksKey))
                result = Barrel.Current.Get<List<Book>>(FetchBooksKey);

            if (result == null)
            {
                result = await _bookAPI.GetBooks();
                Barrel.Current.Add(FetchBooksKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }

            return result;
        }

        public static async Task<Book> FetchBook(string id, bool forceRefresh = false)
        {
            var bookKey = GetSingleBookKey + id;
            Book result = null;

            if (!forceRefresh && Barrel.Current.Exists(bookKey) && !Barrel.Current.IsExpired(bookKey))
                result = Barrel.Current.Get<Book>(bookKey);

            if (result == null)
            {
                result = await _bookAPI.GetBook(id);
                Barrel.Current.Add(bookKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }

            return result;
        }

        #endregion

        #region ADD

        public static async Task<Book> AddBook(Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            return await _bookAPI.AddBook(book);
        }

        #endregion

        #region PATCH

        public static async Task<Book> UpdateBook(Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            return await _bookAPI.UpdateBook(book);
        }

        #endregion

        #region DELETE

        public static async Task<bool> DeleteBook(Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            await _bookAPI.DeleteBook(book);
            return true;
        }

        #endregion
    }
}