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
                result = await _bookShelfWebService.GetList();
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetAllBookShelves));
            }
            return result;
        }

        public async Task<ApiBookShelfResponse> GetNextBookshelves(int page)
        {
            ApiBookShelfResponse result = null;
            try
            {
                result = await _bookShelfWebService.GetList(page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetNextBookshelves));
            }
            return result;
        }

        public async Task<ApiBookShelfDetailResponse> GetBookShelf(string id)
        {
            ApiBookShelfDetailResponse result = null;
            try
            {
                result = await _bookShelfWebService.GetDetail(id);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetBookShelf));
            }
            return result;
        }

        public async Task<ApiBookShelfResponse> SearchBookshelves(string search, int? page = null)
        {
            ApiBookShelfResponse result = null;
            try
            {
                result = await _bookShelfWebService.Search(search, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(SearchBookshelves));
            }
            return result;
        }

        public async Task<bool> AddBookShelf(ApiBookShelfRequest bookshelf)
        {
            ApiBookShelf result = null;
            try
            {
                result = await _bookShelfWebService.Add(bookshelf);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(AddBookShelf));
            }
            return result != null;
        }

        public async Task<ApiBookShelfDetailResponse> UpdateBookShelf(string id, ApiBookShelfRequest bookshelf)
        {
            ApiBookShelfDetailResponse result = null;
            try
            {
                result = await _bookShelfWebService.Update(id, bookshelf);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(UpdateBookShelf));
            }
            return result;
        }

        public async Task<bool> DeleteBookShelf(string id)
        {
            try
            {
                await _bookShelfWebService.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(DeleteBookShelf));
            }
            return false;
        }
    }
}