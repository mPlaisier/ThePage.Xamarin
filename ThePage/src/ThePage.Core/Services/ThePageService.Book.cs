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
                HandleException(ex);
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
                HandleException(ex);
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
                HandleException(ex);
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
                HandleException(ex);
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
                HandleException(ex);
            }
            return false;
        }

        #endregion
    }
}