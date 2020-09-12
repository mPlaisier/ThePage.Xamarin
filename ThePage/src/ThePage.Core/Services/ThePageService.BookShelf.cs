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
    }
}