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
                result = await _authorWebService.GetList();
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetAllAuthors));
            }
            return result;
        }

        public async Task<ApiAuthorResponse> GetNextAuthors(int page)
        {
            ApiAuthorResponse result = null;
            try
            {
                result = await _authorWebService.GetList(page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(GetNextAuthors));
            }
            return result;
        }

        public async Task<ApiAuthor> GetAuthor(string id)
        {
            ApiAuthor result = null;
            try
            {
                result = await _authorWebService.GetDetail(id);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(id));
            }
            return result;
        }

        public async Task<ApiAuthorResponse> SearchAuthors(string search, int? page = null)
        {
            ApiAuthorResponse result = null;
            try
            {
                result = await _authorWebService.Search(search, page);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(SearchAuthors));
            }
            return result;
        }

        public async Task<ApiAuthor> AddAuthor(ApiAuthorRequest author)
        {
            ApiAuthor result = null;
            try
            {
                result = await _authorWebService.Add(author);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(AddAuthor));
            }
            return result;
        }

        public async Task<ApiAuthor> UpdateAuthor(string id, ApiAuthorRequest author)
        {
            ApiAuthor result = null;
            try
            {
                result = await _authorWebService.Update(id, author);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(UpdateAuthor));
            }
            return result;
        }

        public async Task<bool> DeleteAuthor(string id)
        {
            try
            {
                await _authorWebService.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                _exceptionService.HandleThePageException(ex, nameof(DeleteAuthor));
            }
            return false;
        }

        #endregion

    }
}