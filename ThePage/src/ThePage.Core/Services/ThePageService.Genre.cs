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
                _exceptionService.HandleThePageException(ex, "GetAllGenres");
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
                _exceptionService.HandleThePageException(ex, "GetNextGenres");
            }
            return result;
        }

        public async Task<ApiGenre> GetGenre(string id)
        {
            ApiGenre result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Get(token, id);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "GetGenre");
            }
            return result;
        }

        public async Task<ApiGenreResponse> SearchGenres(string search, int? page = null)
        {
            ApiGenreResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Search(token, search, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "SearchGenres");
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
                _exceptionService.HandleThePageException(ex, "AddGenre");
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
                _exceptionService.HandleThePageException(ex, "UpdateGenre");
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
                _exceptionService.HandleThePageException(ex, "DeleteGenre");
            }
            return false;
        }

        #endregion

    }
}
