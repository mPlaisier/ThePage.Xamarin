using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class GenreManager
    {
        const string GENRES_KEY = "GetGenresKey";
        const string GENRES_SINGLE_KEY = "GetGenreKey";

        #region FETCH

        public static async Task<ApiGenreResponse> Get(string token, int? page = null, bool forceRefresh = false)
        {
            var barrelkey = GENRES_KEY + (page ?? 1);

            ApiGenreResponse result = null;
            if (!forceRefresh && Barrel.Current.Exists(barrelkey) && !Barrel.Current.IsExpired(barrelkey))
                result = Barrel.Current.Get<ApiGenreResponse>(barrelkey);

            if (result == null)
            {
                var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
                result = await api.Get(new ApiPageRequest(page));

                Barrel.Current.Add(barrelkey, result, TimeSpan.FromMinutes(Constants.GenreExpirationTimeInMinutes));
            }

            return result;
        }

        public static async Task<ApiGenre> Get(string token, string id, bool forceRefresh = false)
        {
            var genreKey = GENRES_SINGLE_KEY + id;
            ApiGenre result = null;

            if (!forceRefresh && Barrel.Current.Exists(genreKey) && !Barrel.Current.IsExpired(genreKey))
                result = Barrel.Current.Get<ApiGenre>(genreKey);

            if (result == null)
            {
                var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
                result = await api.Get(id);

                Barrel.Current.Add(genreKey, result, TimeSpan.FromMinutes(Constants.GenreExpirationTimeInMinutes));
            }

            return result;
        }

        #endregion

        #region ADD

        public static async Task<ApiGenre> Add(string token, ApiGenreRequest genre)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(GENRES_KEY);

            var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.Add(genre);
        }

        #endregion

        #region PATCH

        public static async Task<ApiGenre> Update(string token, string id, ApiGenreRequest genre)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(GENRES_KEY, GENRES_SINGLE_KEY, genre.Id);

            var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.Update(genre, id);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, ApiGenre genre)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(GENRES_KEY, GENRES_SINGLE_KEY, genre.Id);

            var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            await api.Delete(genre);

            return true;
        }

        #endregion
    }
}