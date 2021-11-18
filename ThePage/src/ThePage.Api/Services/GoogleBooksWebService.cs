using System.Threading.Tasks;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class GoogleBooksWebService : IGoogleBooksWebService
    {
        const string API_KEY = "AIzaSyB4VumwZvL3FuiFd4kYDrF62jdo8QuAauo";

        readonly ITokenlessWebService _webService;

        #region Constructor

        public GoogleBooksWebService(ITokenlessWebService webService)
        {
            _webService = webService;
        }

        #endregion

        #region Public

        public async Task<GoogleBooksResult> SearchByTitle(string title)
        {
            var api = await _webService.GetApi<IGoogleBooksApi>();
            return await api.SearchByTitle(title, API_KEY);
        }

        public async Task<GoogleBooksResult> SearchByIsbn(string isbn)
        {
            var api = await _webService.GetApi<IGoogleBooksApi>();
            return await api.SearchByIsbn(isbn, API_KEY);
        }

        #endregion

    }
}
