using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;
using ThePage.Core.Cells;

namespace ThePage.UnitTests
{
    public static partial class BookDataFactory
    {
        public static Book GetSingleBook()
        {
            return GetSingleFakeBookObject();
        }

        public static BookDetail GetBookDetailWithGenres()
        {
            return new Faker<BookDetail>()
                .RuleFor(b => b.Id, f => Guid.NewGuid().ToString())
                .RuleFor(b => b.Title, f => f.Name.FullName())
                .RuleFor(b => b.Author, f => AuthorDataFactory.GetSingleAuthor())
                .RuleFor(b => b.Owned, f => false)
                .RuleFor(b => b.Read, f => false)
                .RuleFor(b => b.Pages, f => f.Random.Int(0, 1000))
                .RuleFor(b => b.Genres, f => GenreDataFactory.GetGenre4ElementsComplete());
        }

        public static BookDetail GetBookDetailNoGenres()
        {
            return new Faker<BookDetail>()
                .RuleFor(b => b.Id, f => f.Random.Guid().ToString())
                .RuleFor(b => b.Title, f => f.Name.FullName())
                .RuleFor(b => b.Author, f => AuthorDataFactory.GetSingleAuthor())
                .RuleFor(b => b.Owned, f => false)
                .RuleFor(b => b.Read, f => false)
                .RuleFor(b => b.Pages, f => f.Random.Int(0, 1000))
                .RuleFor(b => b.Genres, f => new List<Genre>());
        }

        public static CellBookSelect GetCellBookSelect(bool isSelected = false)
        {
            var book = GetSingleBook();
            return new CellBookSelect(book, isSelected);
        }

        public static IEnumerable<Book> GetListBook4ElementsComplete()
        {
            return GetSingleFakeBookObject().Generate(4);
        }

        public static IEnumerable<Book> GetListBookEmpty()
        {
            return Enumerable.Empty<Book>();
        }

        public static OLObject GetSingleOLObject()
        {
            var olObject = JsonConvert.DeserializeObject<OLObject>(SingleOLObject);
            return olObject;
        }

        public static Faker<Book> GetSingleFakeBookObject()
        {
            return new Faker<Book>()
                .RuleFor(b => b.Id, f => Guid.NewGuid().ToString())
                .RuleFor(b => b.Title, f => f.Name.FullName())
                .RuleFor(b => b.Author, f => AuthorDataFactory.GetSingleAuthor());
        }
    }
}