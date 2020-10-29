using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public partial class ThePageService
    {
        #region public

        public async Task<ApiGenreResponse> GetAllGenres()
        {
            ApiGenreResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Get(token);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<ApiGenreResponse> GetNextGenres(int page)
        {
            ApiGenreResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Get(token, page);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<ApiGenre> AddGenre(ApiGenreRequest genre)
        {
            ApiGenre result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Add(token, genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<ApiGenre> UpdateGenre(string id, ApiGenreRequest genre)
        {
            ApiGenre result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Update(token, id, genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteGenre(ApiGenre genre)
        {
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    return await GenreManager.Delete(token, genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        #endregion

    }
}
