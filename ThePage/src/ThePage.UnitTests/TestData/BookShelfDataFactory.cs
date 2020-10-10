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

        public static ApiBookShelfDetailResponse GetApiBookShelfDetailResponseWithBooks()
        {
            return JsonConvert.DeserializeObject<ApiBookShelfDetailResponse>(BookShelfDetailResponseWithBooks);
        }

        public static ApiBookShelfDetailResponse GetApiBookShelfDetailResponseWithoutBooks()
        {
            return JsonConvert.DeserializeObject<ApiBookShelfDetailResponse>(BookShelfDetailResponseWithoutBooks);
        }

        public static ApiBookShelf GetSingleBookfShelfWithBooks()
        {
            return JsonConvert.DeserializeObject<ApiBookShelf>(SingleBookfShelfWithBooks);
        }

        public static ApiBookShelf GetSingleBookfShelfWithoutBooks()
        {
            return JsonConvert.DeserializeObject<ApiBookShelf>(SingleBookfShelfWithoutBooks);
        }
    }
}