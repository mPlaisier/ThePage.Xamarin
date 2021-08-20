using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    [ThePageLazySingletonService]
    public class OpenLibraryService : IOpenLibraryService
    {
        readonly IExceptionService _exceptionService;

        #region Constructor

        public OpenLibraryService(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
        }

        #endregion

        #region Public

        public async Task<OLObject> GetBookByISBN(string isbn)
        {
            OLObject result = null;
            try
            {
                result = await OpenLibraryManager.Get(isbn);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleOpenLibraryException(ex, "GetBookByISBN", isbn);
            }
            return result;
        }

        #endregion
    }
}