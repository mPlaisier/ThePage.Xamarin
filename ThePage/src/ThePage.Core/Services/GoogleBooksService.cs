using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public class GoogleBooksService : IGoogleBooksService
    {
        readonly IExceptionService _exceptionService;

        #region Constructor

        public GoogleBooksService(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
        }

        #endregion

        #region Public

        public async Task<GoogleBooksResult> SearchBookByTitle(string title)
        {
            GoogleBooksResult results = null;
            try
            {
                results = await GoogleBooksManager.SearchByTitle(title);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleGoogleException(ex, "SearchBookByTitle", title);
            }
            return results;
        }

        public Task<GoogleBooksResult> SearchBookByISBN(string isbn)
        {
            return null;
        }

        #endregion
    }
}