using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public partial class ThePageService
    {
        #region public

        public async Task<ApiAuthorResponse> GetAllAuthors()
        {
            ApiAuthorResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await AuthorManager.Get(token);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> AddAuthor(ApiAuthorRequest author)
        {
            ApiAuthor result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await AuthorManager.Add(token, author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<ApiAuthor> UpdateAuthor(string id, ApiAuthorRequest author)
        {
            ApiAuthor result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await AuthorManager.Update(token, id, author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteAuthor(ApiAuthor author)
        {
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    return await AuthorManager.Delete(token, author);
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