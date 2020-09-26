using Newtonsoft.Json;
using ThePage.Api;

namespace ThePage.UnitTests
{
    public static partial class BookShelfDataFactory
    {
        public static ApiBookShelfResponse GetListBookShelfEmpty()
        {
            return JsonConvert.DeserializeObject<ApiBookShelfResponse>(ListBookShelfEmpty);
        }

        public static ApiBookShelfResponse GetListBookShelf2Elements()
        {
            return JsonConvert.DeserializeObject<ApiBookShelfResponse>(ListBookShelf2Elements);
        }
    }
}