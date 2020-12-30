using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;

namespace ThePage.Api
{
    public class BookManager
    {
        #region CachingKeys

        const string BOOKS_KEY = "GetBooksKey";
        const string BOOKS_SINGLE_KEY = "GetBookKey";

        #endregion

        #region FETCH

        public static async Task<ApiBookResponse> Get(string token, int? page = null, bool forceRefresh = false)
        {
            var barrelkey = BOOKS_KEY + (page ?? 1);

            ApiBookResponse result = null;
            if (!forceRefresh && Barrel.Current.Exists(barrelkey) && !Barrel.Current.IsExpired(barrelkey))
                result = Barrel.Current.Get<ApiBookResponse>(barrelkey);

            if (result == null)
            {
                var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));

                result = await api.GetBooks(new ApiPageRequest(page));
                Barrel.Current.Add(barrelkey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        public static async Task<ApiBookDetailResponse> Get(string token, string id, bool forceRefresh = false)
        {
            var bookKey = BOOKS_SINGLE_KEY + id;
            ApiBookDetailResponse result = null;

            if (!forceRefresh && Barrel.Current.Exists(bookKey) && !Barrel.Current.IsExpired(bookKey))
                result = Barrel.Current.Get<ApiBookDetailResponse>(bookKey);

            if (result == null)
            {
                var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
                result = await api.GetBook(id);

                Barrel.Current.Add(bookKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        #endregion

        #region SEARCH

        public static async Task<ApiBookResponse> SearchTitle(string token, string search, int? page = null)
        {
            ApiBookResponse result = null;

            var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            result = await api.SearchTitle(new ApiSearchRequest(page, search));

            return result;
        }

        #endregion

        #region ADD

        public static async Task<ApiBookDetailRequest> Add(string token, ApiBookDetailRequest book)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(BOOKS_KEY);

            var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            return await api.AddBook(book);
        }

        #endregion

        #region PATCH

        public static async Task<ApiBookDetailResponse> Update(string token, string id, ApiBookDetailRequest book)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(BOOKS_KEY, BOOKS_SINGLE_KEY, id);

            var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            return await api.UpdateBook(book, id);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, string id)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(BOOKS_KEY, BOOKS_SINGLE_KEY, id);
            Barrel.Current.Empty(BookShelfManager.BOOKSHELVES_KEY);

            var api = RestService.For<IBookAPI>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            await api.DeleteBook(id);

            return true;
        }

        #endregion
    }
}