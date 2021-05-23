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
                _exceptionService.HandleThePageException(ex, "GetAllBookShelves");
            }
            return result;
        }

        public async Task<ApiBookShelfResponse> GetNextBookshelves(int page)
        {
            ApiBookShelfResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookShelfManager.Get(token, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "GetNextBookshelves");
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
                _exceptionService.HandleThePageException(ex, "GetBookShelf");
            }
            return result;
        }

        public async Task<ApiBookShelfResponse> SearchBookshelves(string search, int? page = null)
        {
            ApiBookShelfResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookShelfManager.Search(token, search, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "SearchBookshelves");
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
                _exceptionService.HandleThePageException(ex, "AddBookShelf");
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
                _exceptionService.HandleThePageException(ex, "UpdateBookShelf");
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
                _exceptionService.HandleThePageException(ex, "DeleteBookShelf");
            }
            return false;
        }
    }
}