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

        const string FetchBookShelfsKey = "GetBookShelfsKey";
        const string GetSingleBookShelfKey = "GetBookShelfKey";

        #endregion

        #region FETCH

        public static async Task<ApiBookShelfResponse> Get(string token, bool forceRefresh = false)
        {
            ApiBookShelfResponse result = null;
            if (!forceRefresh && !Barrel.Current.Exists(FetchBookShelfsKey) && !Barrel.Current.IsExpired(FetchBookShelfsKey))
                result = Barrel.Current.Get<ApiBookShelfResponse>(FetchBookShelfsKey);

            if (result == null)
            {
                var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));

                result = await api.GetBookShelfs();
                Barrel.Current.Add(FetchBookShelfsKey, result, TimeSpan.FromMinutes(Constants.BookExpirationTimeInMinutes));
            }
            return result;
        }

        #endregion

        #region ADD

        public static async Task<ApiBookShelf> Add(string token, ApiBookShelfRequest bookshelf)
        {
            //Clear cache
            Barrel.Current.Empty(FetchBookShelfsKey);

            var api = RestService.For<IBookShelfApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.AddBookShelf(bookshelf);
        }

        #endregion
    }
}
