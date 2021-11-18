using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    [ThePageLazySingletonService]
    public class OpenLibraryService : IOpenLibraryService
    {
        readonly IExceptionService _exceptionService;
        readonly ITokenlessWebService _webService;

        #region Constructor

        public OpenLibraryService(IExceptionService exceptionService, ITokenlessWebService webService)
        {
            _exceptionService = exceptionService;
            _webService = webService;
        }

        #endregion

        #region Public

        public async Task<OLObject> GetBookByISBN(string isbn)
        {
            try
            {
                var api = await _webService.GetApi<IOpenLibraryApi>();

                var result = await api.Get(isbn);
                return result[$"ISBN:{isbn}"];
            }
            catch (Exception ex)
            {
                _exceptionService.HandleOpenLibraryException(ex, nameof(GetBookByISBN), isbn);
            }
            return null;
        }

        #endregion
    }
}