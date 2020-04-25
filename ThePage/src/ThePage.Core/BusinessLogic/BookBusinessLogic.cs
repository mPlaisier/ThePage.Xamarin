using System.Collections.Generic;
using System.Linq;
using ThePage.Api;
using static ThePage.Core.CellBookInput;

namespace ThePage.Core
{
    public static class BookBusinessLogic
    {
        #region Public

        public static List<CellBook> BooksToCellBooks(List<Book> booksApi, List<Author> authorsApi, List<Genre> genresApi)
        {
            return booksApi?.Select(x => BookToCellBook(x, authorsApi, genresApi)).ToList();
        }

        public static CellBook BookToCellBook(Book book, List<Author> authors, List<Genre> genres)
        {
            var author = AuthorBusinessLogic.GetAuthorFromString(book.Author, authors);
            var bookGenres = GenreBusinessLogic.GetGenresFromString(book.Genres ?? new List<string>(), genres)?.ToList();

            return new CellBook(book, author, bookGenres);
        }

        public static Book CreateBookFromInput(IEnumerable<ICellBook> items, string id = null)
        {
            var title = items.OfType<CellBookTextView>().Where(p => p.InputType == EBookInputType.Title).First().TxtInput.Trim();

            var author = items.OfType<CellBookAuthor>().Where(p => p.InputType == EBookInputType.Author).First().SelectedAuthor;

            var genres = items.OfType<CellBookGenreItem>().Select(i => i.Genre);

            var isbn = items.OfType<CellBookTextView>().Where(p => p.InputType == EBookInputType.ISBN).First().TxtInput;

            var owned = items.OfType<CellBookSwitch>().Where(p => p.InputType == EBookInputType.Owned).First().IsSelected;

            var read = items.OfType<CellBookSwitch>().Where(p => p.InputType == EBookInputType.Read).First().IsSelected;

            var pages = items.OfType<CellBookNumberTextView>().Where(p => p.InputType == EBookInputType.Pages).First().TxtNumberInput;

            return new Book(id, title, author.Id, genres.GetIdStrings(), isbn, owned, read, pages);
        }


        #endregion
    }
}
