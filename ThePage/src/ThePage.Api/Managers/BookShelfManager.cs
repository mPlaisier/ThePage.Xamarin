using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class BookShelfManager
    {
        #region CachingKeys

        public const string FetchBookShelvesKey = "GetBookShelvesKey";
        const string GetSingleBookShelfKey = "GetBookShelfKey";

        #endregion

        #region FETCH

        public static async Task<ApiBookShelfResponse> Get(string token, bool forceRefresh = false)
        {
            ApiBookShelfResponse result = null;
            if (!forceRefresh && !Barrel.Current.Exists(FetchBookShelvesKey) && !Barrel.Current.IsExpired(FetchBookShelvesKey))
                result = Barrel.Current.Get<ApiBookShelfResponse>(FetchBookShelvesKey);

            if (result == null)
            {
                var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));

                result = await api.GetBookShelves();
                Barrel.Current.Add(FetchBookShelvesKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        public static async Task<ApiBookShelfDetailResponse> Get(string token, string id, bool forceRefresh = false)
        {
            var bookShelfKey = GetSingleBookShelfKey + id;
            ApiBookShelfDetailResponse result = null;

            if (!forceRefresh && Barrel.Current.Exists(bookShelfKey) && !Barrel.Current.IsExpired(bookShelfKey))
                result = Barrel.Current.Get<ApiBookShelfDetailResponse>(bookShelfKey);

            if (result == null)
            {
                var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
                result = await api.GetBookShelf(id);

                Barrel.Current.Add(bookShelfKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        #endregion

        #region ADD

        public static async Task<ApiBookShelf> Add(string token, ApiBookShelfRequest bookshelf)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBookShelvesKey);

            var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.AddBookShelf(bookshelf);
        }

        #endregion

        #region PATCH

        public static async Task<ApiBookShelfDetailResponse> Update(string token, string id, ApiBookShelfRequest bookShelf)
        {
            //Clear cache
            var bookShelfKey = GetSingleBookShelfKey + id;
            Barrel.Current.Empty(bookShelfKey);
            Barrel.Current.Empty(FetchBookShelvesKey);

            var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.UpdateBookShelf(bookShelf, id);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, ApiBookShelfDetailResponse bookShelf)
        {
            //Clear cache
            var bookShelfKey = GetSingleBookShelfKey + bookShelf.Id;
            Barrel.Current.Empty(bookShelfKey);
            Barrel.Current.Empty(FetchBookShelvesKey);

            var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            await api.DeleteBookShelf(bookShelf);

            return true;
        }

        #endregion
    }
}
