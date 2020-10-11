using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public partial class ThePageService
    {
        public async Task<ApiBookShelfResponse> GetAllBookShelves()
        {
            ApiBookShelfResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookShelfManager.Get(token);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<ApiBookShelfDetailResponse> GetBookShelf(string id)
        {
            ApiBookShelfDetailResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookShelfManager.Get(token, id);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> AddBookShelf(ApiBookShelfRequest bookshelf)
        {
            ApiBookShelf result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookShelfManager.Add(token, bookshelf);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<ApiBookShelfDetailResponse> UpdateBookShelf(string id, ApiBookShelfRequest bookshelf)
        {
            ApiBookShelfDetailResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookShelfManager.Update(token, id, bookshelf);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteBookShelf(string id)
        {
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    return await BookShelfManager.Delete(token, id);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }
    }
}