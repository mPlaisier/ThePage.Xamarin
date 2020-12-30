using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public class OpenLibraryManager
    {
        static readonly IOpenLibraryAPI _olLibrary = RestService.For<IOpenLibraryAPI>(Constants.OpenLibrary_Api_Url);

        #region FETCH

        public static async Task<OLObject> Get(string isbn)
        {
            var result = await _olLibrary.Get(isbn);
            return result[$"ISBN:{isbn}"];
        }

        #endregion

    }
}
