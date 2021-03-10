using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public static class OpenLibraryManager
    {
        static readonly IOpenLibraryApi _olLibrary = RestService.For<IOpenLibraryApi>(Constants.OpenLibrary_Api_Url);

        #region FETCH

        public static async Task<OLObject> Get(string isbn)
        {
            var result = await _olLibrary.Get(isbn);
            return result[$"ISBN:{isbn}"];
        }

        #endregion

    }
}
