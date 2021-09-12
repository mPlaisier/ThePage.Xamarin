using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class BookShelfBusinessLogic
    {
        public static (ApiBookShelfRequest request, IEnumerable<Book> books) CreateApiBookShelfRequestFromInput(IEnumerable<ICell> items, string id = null, BookshelfDetail originalResponse = null)
        {
            //Name
            var name = items.OfType<CellBookShelfTextView>().First(p => p.InputType == EBookShelfInputType.Name).TxtInput.Trim();
            if (name == null || name.Equals(originalResponse?.Name))
                name = null;

            //Books
            var books = items.OfType<CellBookShelfBookItem>().Select(i => i.Book).ToList();
            if (books == null || (books.Count == originalResponse?.Books.Count && !books.Except(originalResponse?.Books).Any()))
                books = null;

            return (name == null && books == null
                ? null
                : new ApiBookShelfRequest(id, name, books.GetIdList(true)), books);
        }

        public static IEnumerable<Bookshelf> MapBookshelves(IEnumerable<ApiBookShelf> bookShelves)
        {
            return bookShelves.Select(bookshelf => MapBookshelf(bookshelf));
        }

        public static Bookshelf MapBookshelf(ApiBookShelf bookshelf)
        {
            return new Bookshelf()
            {
                Id = bookshelf.Id,
                Name = bookshelf.Name,
                Books = bookshelf.Books
            };
        }

        public static BookshelfDetail MapBookshelfDetail(ApiBookShelfDetailResponse bookshelf)
        {
            return new BookshelfDetail()
            {
                Id = bookshelf.Id,
                Name = bookshelf.Name,
                Books = BookBusinessLogic.MapBooks(bookshelf.Books).ToList()
            };
        }
    }
}