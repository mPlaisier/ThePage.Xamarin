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
                result = await _bookWebService.GetList();
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetAllBooks));
            }
            return result;
        }

        public async Task<ApiBookResponse> GetNextBooks(int page)
        {
            ApiBookResponse result = null;
            try
            {
                result = await _bookWebService.GetList(page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetNextBooks));
            }
            return result;
        }

        public async Task<ApiBookDetailResponse> GetBook(string id)
        {
            ApiBookDetailResponse result = null;
            try
            {
                result = await _bookWebService.GetDetail(id);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetBook));
            }
            return result;
        }

        public async Task<ApiBookResponse> SearchBooksTitle(string search, int? page = null)
        {
            ApiBookResponse result = null;
            try
            {
                result = await _bookWebService.SearchTitle(search, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(SearchBooksTitle));
            }
            return result;
        }

        public async Task<ApiBookDetailResponse> AddBook(ApiBookDetailRequest book)
        {
            ApiBookDetailResponse result = null;
            try
            {
                result = await _bookWebService.Add(book);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(AddBook));
            }
            return result;
        }

        public async Task<ApiBookDetailResponse> UpdateBook(string id, ApiBookDetailRequest book)
        {
            ApiBookDetailResponse result = null;
            try
            {
                result = await _bookWebService.Update(id, book);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(UpdateBook));
            }
            return result;
        }

        public async Task<bool> DeleteBook(string id)
        {
            try
            {
                await _bookWebService.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(DeleteBook));
            }
            return false;
        }

        #endregion
    }
}