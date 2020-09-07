using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class AuthorDataFactory
    {
        public static ApiAuthorResponse GetListAuthor4ElementsComplete()
        {
            return JsonConvert.DeserializeObject<ApiAuthorResponse>(ListAuthor4ElementsComplete);
        }

        public static ApiAuthorResponse GetListAuthorEmpty()
        {
            return JsonConvert.DeserializeObject<ApiAuthorResponse>(ListAuthorDataEmpty);
        }

        public static ApiAuthor GetSingleAuthor()
        {
            return JsonConvert.DeserializeObject<ApiAuthor>(SingleAuthor);
        }
    }
}
