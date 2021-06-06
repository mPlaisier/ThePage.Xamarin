using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public static class GoogleBooksManager
    {
        const string API_KEY = "AIzaSyB4VumwZvL3FuiFd4kYDrF62jdo8QuAauo";

        #region Public

        public static async Task<GoogleBooksResult> SearchByTitle(string title)
        {
            var googleBooksApi = RestService.For<IGoogleBooksApi>(HttpUtils.GetHttpClient(Constants.Google_Books_Url));
            var result = await googleBooksApi.SearchByTitle(title, API_KEY);

            return result;
        }

        public static async Task<GoogleBooksResult> SearchByIsbn(string isbn)
        {
            var googleBooksApi = RestService.For<IGoogleBooksApi>(HttpUtils.GetHttpClient(Constants.Google_Books_Url));
            var result = await googleBooksApi.SearchByIsbn(isbn, API_KEY);

            return result;
        }

        #endregion
    }
}