using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public class GenreManager
    {
        #region Properties

        static readonly IGenreAPI _genreApi = RestService.For<IGenreAPI>(Constants.ThePageAPI_Url);

        #endregion


        #region FETCH

        public static async Task<List<Genre>> Get()
        {
            return await _genreApi.Get();
        }

        public static async Task<Genre> Get(string id)
        {
            return await _genreApi.Get(id);
        }

        #endregion

        #region ADD

        public static async Task<Genre> Add(Genre genre)
        {
            return await _genreApi.Add(genre);
        }

        #endregion

        #region PATCH

        public static async Task<Genre> Update(Genre genre)
        {
            return await _genreApi.Update(genre);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(Genre genre)
        {
            //TODO improve the API to return a better result
            //ATM we receive if successfull:
            //"{\"message\":\"Deleted genre\"}"
            await _genreApi.Delete(genre);

            return true;
        }

        #endregion
    }
}