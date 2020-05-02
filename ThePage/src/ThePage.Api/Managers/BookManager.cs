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
        #region Properties

        static readonly IBookAPI _bookAPI = RestService.For<IBookAPI>(Secrets.ThePageAPI_URL);

        #endregion

        #region FETCH

        public static async Task<List<Book>> FetchBooks(bool forceRefresh = false)
        {
            Barrel.EncryptionKey = "encryptionKey";
            var fetchBookKey = "FetchBookKey";

            //check internet
            List<Book> result = null;
            var exists = Barrel.Current.Exists(fetchBookKey);
            var expired = Barrel.Current.GetExpiration("doesnt exists");
            var isexpired = Barrel.Current.IsExpired(fetchBookKey);


            if (!forceRefresh && !Barrel.Current.Exists(fetchBookKey) && !Barrel.Current.IsExpired(key: fetchBookKey))
            {
                result = Barrel.Current.Get<List<Book>>(key: fetchBookKey);
            }

            if (result == null)
            {
                result = await _bookAPI.GetBooks();
                Barrel.Current.Add(fetchBookKey, result, TimeSpan.FromMinutes(5));
            }

            return result;
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