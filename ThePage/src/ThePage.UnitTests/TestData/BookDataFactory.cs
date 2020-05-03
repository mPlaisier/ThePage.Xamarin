using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class BookDataFactory
    {
        public static List<Book> GetListBook4ElementsComplete()
        {
            return JsonConvert.DeserializeObject<List<Book>>(ListBook4ElementsComplete);
        }

        public static List<Book> GetListBookEmpty()
        {
            return JsonConvert.DeserializeObject<List<Book>>(ListBookDataEmpty);
        }

        public static Book GetSingleBook()
        {
            return JsonConvert.DeserializeObject<Book>(SingleBook);
        }

        public static CellBook GetSingleCellBook()
        {
            var book = GetSingleBook();
            var author = AuthorDataFactory.GetSingleAuthor();
            var genres = GenreDataFactory.GetListGenre4ElementsComplete();
            return new CellBook(book, author, genres);
        }

        public static OLObject GetSingleOLObject()
        {
            var olObject = JsonConvert.DeserializeObject<OLObject>(SingleOLObject);
            return olObject;
        }
    }
}
