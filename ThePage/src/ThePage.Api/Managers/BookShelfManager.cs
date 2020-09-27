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

        const string FetchBookShelvesKey = "GetBookShelvesKey";
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

                result = await api.GetBookShelfs();
                Barrel.Current.Add(FetchBookShelvesKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
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
    }
}
