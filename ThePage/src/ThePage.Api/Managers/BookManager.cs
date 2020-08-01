using System;
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
        const string GetSingleBookKey = "GetBookKey";

        #endregion

        #region FETCH

        public static async Task<ApiBookResponse> Get(string token, bool forceRefresh = false)
        {
            ApiBookResponse result = null;
            if (!forceRefresh && !Barrel.Current.Exists(FetchBooksKey) && !Barrel.Current.IsExpired(FetchBooksKey))
                result = Barrel.Current.Get<ApiBookResponse>(FetchBooksKey);

            if (result == null)
            {
                var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));

                result = await api.GetBooks();
                Barrel.Current.Add(FetchBooksKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        public static async Task<ApiBookDetailResponse> Get(string token, string id, bool forceRefresh = false)
        {
            var bookKey = GetSingleBookKey + id;
            ApiBookDetailResponse result = null;

            if (!forceRefresh && Barrel.Current.Exists(bookKey) && !Barrel.Current.IsExpired(bookKey))
                result = Barrel.Current.Get<ApiBookDetailResponse>(bookKey);

            if (result == null)
            {
                var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
                result = await api.GetBook(id);

                Barrel.Current.Add(bookKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        #endregion

        #region ADD

        public static async Task<Book> Add(string token, Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.AddBook(book);
        }

        #endregion

        #region PATCH

        public static async Task<Book> Update(string token, Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.UpdateBook(book);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, Book book)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBooksKey);

            var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            await api.DeleteBook(book);

            return true;
        }

        #endregion
    }
}