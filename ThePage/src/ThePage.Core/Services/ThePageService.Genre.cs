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
                result = await _genreWebService.GetList();
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetAllGenres));
            }
            return result;
        }

        public async Task<ApiGenreResponse> GetNextGenres(int page)
        {
            ApiGenreResponse result = null;
            try
            {
                result = await _genreWebService.GetList(page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetNextGenres));
            }
            return result;
        }

        public async Task<ApiGenre> GetGenre(string id)
        {
            ApiGenre result = null;
            try
            {
                result = await _genreWebService.GetDetail(id);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetGenre));
            }
            return result;
        }

        public async Task<ApiGenreResponse> SearchGenres(string search, int? page = null)
        {
            ApiGenreResponse result = null;
            try
            {
                result = await _genreWebService.Search(search, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(SearchGenres));
            }
            return result;
        }

        public async Task<ApiGenre> AddGenre(ApiGenreRequest genre)
        {
            ApiGenre result = null;
            try
            {
                result = await _genreWebService.Add(genre);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(genre));
            }
            return result;
        }

        public async Task<ApiGenre> UpdateGenre(string id, ApiGenreRequest genre)
        {
            ApiGenre result = null;
            try
            {
                result = await _genreWebService.Update(id, genre);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(UpdateGenre));
            }
            return result;
        }

        public async Task<bool> DeleteGenre(string id)
        {
            try
            {
                await _genreWebService.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(DeleteGenre));
            }
            return false;
        }

        #endregion

    }
}
