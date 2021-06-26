using System.Collections.Generic;
using System.Linq;
using Bogus;
using Moq;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class BookShelfDataFactory
    {
        public static Bookshelf GetSingleBookfShelfWithBooks()
        {
            return GetFakerBookObjectWithBooks().Generate();
        }

        public static IEnumerable<Bookshelf> GetListBookShelf2Elements()
        {
            return GetFakerBookObjectWithBooks().Generate(2);
        }

        public static Bookshelf GetSingleBookfShelfWithoutBooks()
        {
            return GetFakerBookObjectWithoutBooks().Generate();
        }

        public static BookshelfDetail GetBookShelfDetailWithBooks()
        {
            return new BookshelfDetail
            {
                Id = It.IsAny<string>(),
                Name = It.IsAny<string>(),
                Books = BookDataFactory.GetSingleFakeBookObject().Generate(4)
            };
        }

        public static BookshelfDetail GetBookShelfDetailWithoutBooks()
        {
            return new BookshelfDetail
            {
                Id = It.IsAny<string>(),
                Name = It.IsAny<string>(),
                Books = new List<Book>()
            };
        }

        public static IEnumerable<Bookshelf> GetListBookShelfEmpty()
        {
            return Enumerable.Empty<Bookshelf>();
        }

        #region Private

        static Faker<Bookshelf> GetFakerBookObjectWithBooks()
        {
            return new Faker<Bookshelf>()
                .RuleFor(b => b.Id, f => f.Random.Guid().ToString())
                .RuleFor(b => b.Name, f => f.Name.FirstName())
                .RuleFor(b => b.Books, f => f.Make(3, () => f.Random.Guid().ToString()));
        }

        static Faker<Bookshelf> GetFakerBookObjectWithoutBooks()
        {
            return new Faker<Bookshelf>()
                .RuleFor(b => b.Id, f => f.Random.Guid().ToString())
                .RuleFor(b => b.Name, f => f.Name.FirstName())
                .RuleFor(b => b.Books, f => new List<string>());
        }

        #endregion
    }
}