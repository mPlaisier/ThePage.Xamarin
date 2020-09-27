using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class BookDataFactory
    {
        public static ApiBookResponse GetListBook4ElementsComplete()
        {
            return JsonConvert.DeserializeObject<ApiBookResponse>(ListBook4ElementsComplete);
        }

        public static ApiBookResponse GetListBook1ElementsComplete()
        {
            return JsonConvert.DeserializeObject<ApiBookResponse>(ListBook1ElementsComplete);
        }

        public static ApiBookResponse GetListBookEmpty()
        {
            return JsonConvert.DeserializeObject<ApiBookResponse>(ListBookDataEmpty);
        }

        public static ApiBookDetailResponse GetSingleBook()
        {
            return JsonConvert.DeserializeObject<ApiBookDetailResponse>(SingleBookWithGenres);
        }

        public static ApiBook GetSingleApiBook()
        {
            return JsonConvert.DeserializeObject<ApiBook>(SingleApiBook);
        }

        public static ApiBookDetailResponse GetApiBookDetailResponseWithGenres()
        {
            return JsonConvert.DeserializeObject<ApiBookDetailResponse>(ApiBookDetailResponseWithGenres);
        }

        public static ApiBookDetailResponse GetApiBookDetailResponseNoGenres()
        {
            return JsonConvert.DeserializeObject<ApiBookDetailResponse>(ApiBookDetailResponseNoGenres);
        }

        public static OLObject GetSingleOLObject()
        {
            var olObject = JsonConvert.DeserializeObject<OLObject>(SingleOLObject);
            return olObject;
        }

        public static CellBookSelect GetCellBookSelect(bool isSelected = false)
        {
            var book = GetSingleApiBook();
            return new CellBookSelect(book, isSelected);
        }
    }
}
