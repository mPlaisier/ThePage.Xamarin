using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class GenreManager
    {
        const string GetGenresKey = "GetGenresKey";
        const string GetSingleGenreKey = "GetGenreKey";

        #region FETCH

        public static async Task<ApiGenreResponse> Get(string token, bool forceRefresh = false)
        {
            ApiGenreResponse result = null;
            if (!forceRefresh && Barrel.Current.Exists(GetGenresKey) && !Barrel.Current.IsExpired(GetGenresKey))
                result = Barrel.Current.Get<ApiGenreResponse>(GetGenresKey);

            if (result == null)
            {
                var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
                result = await api.Get();

                Barrel.Current.Add(GetGenresKey, result, TimeSpan.FromMinutes(Constants.GenreExpirationTimeInMinutes));
            }

            return result;
        }

        public static async Task<ApiGenre> Get(string token, string id, bool forceRefresh = false)
        {
            var genreKey = GetSingleGenreKey + id;
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
            Barrel.Current.Empty(GetGenresKey);

            var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.Add(genre);
        }

        #endregion

        #region PATCH

        public static async Task<ApiGenre> Update(string token, string id, ApiGenreRequest genre)
        {
            //Clear cache
            var genreKey = GetSingleGenreKey + id;
            Barrel.Current.Empty(genreKey);
            Barrel.Current.Empty(GetGenresKey);

            var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.Update(genre, id);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, ApiGenre genre)
        {
            //Clear cache
            var genreKey = GetSingleGenreKey + genre.Id;
            Barrel.Current.Empty(genreKey);
            Barrel.Current.Empty(GetGenresKey);

            var api = RestService.For<IGenreAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            await api.Delete(genre);

            return true;
        }

        #endregion
    }
}