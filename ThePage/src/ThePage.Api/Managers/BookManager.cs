using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class BookManager
    {
        #region CachingKeys

        const string FetchBooksKey = "GetBooksKey";
        const string FetchBooksV2Key = "GetBooksV2Key";
        const string GetSingleBookKey = "GetBookKey";

        #endregion

        #region Properties

        static readonly IBookAPI _bookAPI = RestService.For<IBookAPI>(Secrets.ThePageAPI_URL);

        #endregion

        #region FETCH

        public static async Task<List<Book>> Get(bool forceRefresh = false)
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

        public static async Task<Book> Get(string id, bool forceRefresh = false)
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

        public static async Task<ApiBookResponse> GetV2(string token, bool forceRefresh = false)
        {
            ApiBookResponse result = null;
            if (!forceRefresh && !Barrel.Current.Exists(FetchBooksV2Key) && !Barrel.Current.IsExpired(FetchBooksV2Key))
                result = Barrel.Current.Get<ApiBookResponse>(FetchBooksV2Key);

            if (result == null)
            {
                var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));

                result = await api.GetBooksV2();
                Barrel.Current.Add(FetchBooksV2Key, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }

            return result;
        }

        #endregion

        #region ADD

        public static async Task<Book> Add(Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            return await _bookAPI.AddBook(book);
        }

        #endregion

        #region PATCH

        public static async Task<Book> Update(Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            return await _bookAPI.UpdateBook(book);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            await _bookAPI.DeleteBook(book);
            return true;
        }

        #endregion
    }
}