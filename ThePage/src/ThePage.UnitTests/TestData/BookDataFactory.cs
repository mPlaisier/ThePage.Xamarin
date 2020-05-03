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
            return JsonConvert.DeserializeObject<Book>(SingleBookWithGenres);
        }

        public static CellBook GetSingleCellBookWith2Genres()
        {
            var book = GetSingleBook();
            var author = JsonConvert.DeserializeObject<Author>(SingleBookAuthor);
            var genres = JsonConvert.DeserializeObject<List<Genre>>(SingleBookGenres);
            return new CellBook(book, author, genres);
        }

        public static CellBook GetSingleCellBookWithoutGenres()
        {
            var book = GetSingleBook();
            var author = JsonConvert.DeserializeObject<Author>(SingleBookAuthor);
            var genres = new List<Genre>();
            return new CellBook(book, author, genres);
        }

        public static OLObject GetSingleOLObject()
        {
            var olObject = JsonConvert.DeserializeObject<OLObject>(SingleOLObject);
            return olObject;
        }
    }
}
