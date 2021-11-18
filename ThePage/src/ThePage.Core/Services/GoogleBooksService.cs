using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    [ThePageLazySingletonService]
    public class GoogleBooksService : IGoogleBooksService
    {
        readonly IExceptionService _exceptionService;
        readonly IGoogleBooksWebService _googleBooksWebService;

        #region Constructor

        public GoogleBooksService(IExceptionService exceptionService, IGoogleBooksWebService googleBooksWebService)
        {
            _exceptionService = exceptionService;
            _googleBooksWebService = googleBooksWebService;
        }

        #endregion

        #region Public

        public async Task<GoogleBooksResult> SearchBookByTitle(string title)
        {
            GoogleBooksResult results = null;
            try
            {
                results = await _googleBooksWebService.SearchByTitle(title);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleGoogleException(ex, "SearchBookByTitle", title);
            }
            return results;
        }

        public async Task<GoogleBooksResult> SearchBookByISBN(string isbn)
        {
            GoogleBooksResult results = null;
            try
            {
                results = await _googleBooksWebService.SearchByIsbn(isbn);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleGoogleException(ex, "SearchBookByTitle", isbn);
            }
            return results;
        }

        #endregion
    }
}