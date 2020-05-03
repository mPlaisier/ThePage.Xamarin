using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class AuthorDataFactory
    {
        public static List<Author> GetListAuthor4ElementsComplete()
        {
            return JsonConvert.DeserializeObject<List<Author>>(ListAuthor4ElementsComplete);
        }

        public static List<Author> GetListAuthorEmpty()
        {
            return JsonConvert.DeserializeObject<List<Author>>(ListAuthorDataEmpty);
        }

        public static Author GetSingleAuthor()
        {
            return JsonConvert.DeserializeObject<Author>(SingleAuthor);
        }

        public static CellAuthor GetSingleCellAuthor()
        {
            var Author = GetSingleAuthor();
            return new CellAuthor(Author);
        }
    }
}
