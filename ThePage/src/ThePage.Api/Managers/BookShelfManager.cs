using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;

namespace ThePage.Api
{
    public class BookShelfManager
    {
        #region CachingKeys

        public const string BOOKSHELVES_KEY = "GetBookShelvesKey";
        const string BOOKSHELVES_SINGLE_KEY = "GetBookShelfKey";

        #endregion

        #region FETCH

        public static async Task<ApiBookShelfResponse> Get(string token, int? page = null, bool forceRefresh = false)
        {
            var barrelkey = BOOKSHELVES_KEY + (page ?? 1);

            ApiBookShelfResponse result = null;
            if (!forceRefresh && Barrel.Current.Exists(barrelkey) && !Barrel.Current.IsExpired(barrelkey))
                result = Barrel.Current.Get<ApiBookShelfResponse>(barrelkey);

            if (result == null)
            {
                var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));

                result = await api.GetBookShelves(new ApiPageRequest(page));
                Barrel.Current.Add(barrelkey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        public static async Task<ApiBookShelfDetailResponse> Get(string token, string id, bool forceRefresh = false)
        {
            var bookShelfKey = BOOKSHELVES_SINGLE_KEY + id;
            ApiBookShelfDetailResponse result = null;

            if (!forceRefresh && Barrel.Current.Exists(bookShelfKey) && !Barrel.Current.IsExpired(bookShelfKey))
                result = Barrel.Current.Get<ApiBookShelfDetailResponse>(bookShelfKey);

            if (result == null)
            {
                var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
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
            ManagerUtils.ClearPageBarrels(BOOKSHELVES_KEY);

            var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            return await api.AddBookShelf(bookshelf);
        }

        #endregion

        #region PATCH

        public static async Task<ApiBookShelfDetailResponse> Update(string token, string id, ApiBookShelfRequest bookShelf)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(BOOKSHELVES_KEY, BOOKSHELVES_SINGLE_KEY, id);

            var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            return await api.UpdateBookShelf(bookShelf, id);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, string id)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(BOOKSHELVES_KEY, BOOKSHELVES_SINGLE_KEY, id);

            var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            await api.DeleteBookShelf(id);

            return true;
        }

        #endregion
    }
}
