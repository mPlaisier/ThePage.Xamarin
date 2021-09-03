using System;
using Newtonsoft.Json;
using ThePage.Api;

namespace ThePage.UnitTests
{
    public static partial class ApiBookDataFactory
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

        public static ApiBookDetailResponse GetSingleApiBookDetailResponse()
        {
            return JsonConvert.DeserializeObject<ApiBookDetailResponse>(SingleBookWithGenres);
        }

        public static ApiBook GetSingleApiBook()
        {
            return JsonConvert.DeserializeObject<ApiBook>(SingleApiBook);
        }

    }
}
