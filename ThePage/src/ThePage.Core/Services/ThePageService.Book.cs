using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public partial class ThePageService
    {
        #region Public

        public async Task<ApiBookResponse> GetAllBooks()
        {
            ApiBookResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Get(token);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "GetAllBooks");
            }
            return result;
        }

        public async Task<ApiBookResponse> GetNextBooks(int page)
        {
            ApiBookResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Get(token, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "GetNextBooks");
            }
            return result;
        }

        public async Task<ApiBookDetailResponse> GetBook(string id)
        {
            ApiBookDetailResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Get(token, id);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "GetBook");
            }
            return result;
        }

        public async Task<ApiBookResponse> SearchBooksTitle(string search, int? page = null)
        {
            ApiBookResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.SearchTitle(token, search, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "SearchBooksTitle");
            }
            return result;
        }

        public async Task<ApiBookDetailRequest> AddBook(ApiBookDetailRequest book)
        {
            ApiBookDetailRequest result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Add(token, book);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "AddBook");
            }
            return result;
        }

        public async Task<ApiBookDetailResponse> UpdateBook(string id, ApiBookDetailRequest book)
        {
            ApiBookDetailResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Update(token, id, book);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "UpdateBook");
            }
            return result;
        }

        public async Task<bool> DeleteBook(string id)
        {
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    return await BookManager.Delete(token, id);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, "DeleteBook");
            }
            return false;
        }

        #endregion
    }
}