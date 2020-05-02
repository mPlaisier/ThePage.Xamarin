using System;
using System.Collections.Generic;
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

        #region Properties

        static readonly IGenreAPI _genreApi = RestService.For<IGenreAPI>(Secrets.ThePageAPI_URL);

        #endregion

        #region FETCH

        public static async Task<List<Genre>> Get(bool forceRefresh = false)
        {
            List<Genre> result = null;
            if (!forceRefresh && Barrel.Current.Exists(GetGenresKey) && !Barrel.Current.IsExpired(GetGenresKey))
                result = Barrel.Current.Get<List<Genre>>(GetGenresKey);

            if (result == null)
            {
                result = await _genreApi.Get();
                Barrel.Current.Add(GetGenresKey, result, TimeSpan.FromMinutes(Constants.GenreExpirationTimeInMinutes));
            }

            return result;
        }

        public static async Task<Genre> Get(string id, bool forceRefresh = false)
        {
            var genreKey = GetSingleGenreKey + id;
            Genre result = null;

            if (!forceRefresh && Barrel.Current.Exists(genreKey) && !Barrel.Current.IsExpired(genreKey))
                result = Barrel.Current.Get<Genre>(genreKey);

            if (result == null)
            {
                result = await _genreApi.Get(id);
                Barrel.Current.Add(genreKey, result, TimeSpan.FromMinutes(Constants.GenreExpirationTimeInMinutes));
            }

            return result;
        }

        #endregion

        #region ADD

        public static async Task<Genre> Add(Genre genre)
        {
            //Clear cache
            Barrel.Current.Empty(GetGenresKey);

            return await _genreApi.Add(genre);
        }

        #endregion

        #region PATCH

        public static async Task<Genre> Update(Genre genre)
        {
            //Clear cache
            Barrel.Current.Empty(GetGenresKey);

            return await _genreApi.Update(genre);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(Genre genre)
        {
            //Clear cache
            Barrel.Current.Empty(GetGenresKey);

            //TODO improve the API to return a better result
            //ATM we receive if successfull:
            //"{\"message\":\"Deleted genre\"}"
            await _genreApi.Delete(genre);

            return true;
        }

        #endregion
    }
}