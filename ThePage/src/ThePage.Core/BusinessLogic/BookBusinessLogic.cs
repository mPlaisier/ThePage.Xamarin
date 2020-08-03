using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
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

        public static (ApiBookDetailRequest, ApiAuthor author, IEnumerable<ApiGenre>) CreateBookFromInput(IEnumerable<ICellBook> items, string id = null, ApiBookDetailResponse originalResponse = null)
        {
            //Title
            var title = items.OfType<CellBookTextView>().Where(p => p.InputType == EBookInputType.Title).First().TxtInput.Trim();
            if (title == null || title.Equals(originalResponse?.Title))
                title = null;

            //Author
            var author = items.OfType<CellBookAuthor>().Where(p => p.InputType == EBookInputType.Author).First().Item;
            if (author == null || author.Equals(originalResponse?.Author.Id))
                author = null;

            //Genres
            var genres = items.OfType<CellBookGenreItem>().Select(i => i.Genre);
            if (genres == null || genres.Equals(originalResponse?.Genres))
                genres = null;

            //Isbn
            var isbn = items.OfType<CellBookTextView>().Where(p => p.InputType == EBookInputType.ISBN).First().TxtInput;
            if (isbn == null || isbn.Equals(originalResponse?.ISBN))
                isbn = null;

            //Owned
            bool? owned = items.OfType<CellBookSwitch>().Where(p => p.InputType == EBookInputType.Owned).First().IsSelected;
            if (owned == null || owned == originalResponse?.Owned)
                owned = null;

            //Read
            bool? read = items.OfType<CellBookSwitch>().Where(p => p.InputType == EBookInputType.Read).First().IsSelected;
            if (read == null || title.Equals(originalResponse?.Title))
                read = null;

            //Pages
            int? pages = items.OfType<CellBookNumberTextView>().Where(p => p.InputType == EBookInputType.Pages).First().TxtNumberInput;
            if (pages == null || title.Equals(originalResponse?.Title))
                pages = null;

            //Build
            return (new ApiBookDetailRequest
                       .Builder()
                       .SetId(id)
                       .SetTitle(title)
                       .SetAuthor(author.Id)
                       .SetGenres(genres.GetIdStrings(true))
                       .SetIsbn(isbn)
                       .SetOwned(owned)
                       .SetRead(read)
                       .SetPages(pages)
                       .Build(),
                    author,
                    genres);
        }

        #endregion

        #region Public BookDetail

        public static MvxObservableCollection<ICellBook> CreateCellBookDetailCells(ApiBookDetailResponse response,
                                                                     Action updateValidation,
                                                                     Action<CellBookGenreItem> removeGenre,
                                                                     Func<Task> deleteBook,
                                                                     IMvxNavigationService navigation,
                                                                     IDevice device)
        {
            var items = new MvxObservableCollection<ICellBook>
            {
                new CellBookTextView("Title",response.Title, EBookInputType.Title,updateValidation),
                new CellBookAuthor(response.Author,navigation, device, updateValidation),
                new CellBookTitle("Genres")
            };

            foreach (var item in response.Genres)
            {
                items.Add(new CellBookGenreItem(item, removeGenre));
            }

            items.Add(new CellBookNumberTextView("Pages", response.Pages.ToString(), EBookInputType.Pages, updateValidation, true));
            items.Add(new CellBookNumberTextView("ISBN", response.ISBN, EBookInputType.ISBN, updateValidation, false));
            items.Add(new CellBookSwitch("Do you own this book?", response.Owned, EBookInputType.Owned, updateValidation));
            items.Add(new CellBookSwitch("Have you read this book?", response.Read, EBookInputType.Read, updateValidation));
            items.Add(new CellBookButton("Delete Book", deleteBook, false));

            return items;
        }

        #endregion
    }
}