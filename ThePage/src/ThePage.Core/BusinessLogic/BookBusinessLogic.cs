using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using ThePage.Api;
using static ThePage.Core.CellBookInput;

namespace ThePage.Core
{
    public static class BookBusinessLogic
    {
        #region Public

        public static (ApiBookDetailRequest request, ApiAuthor author, IEnumerable<ApiGenre> genres) CreateBookDetailRequestFromInput(IEnumerable<ICellBook> items, string id = null, ApiBookDetailResponse originalResponse = null)
        {
            //Title
            var title = items.OfType<CellBookTextView>().Where(p => p.InputType == EBookInputType.Title).First().TxtInput.Trim();
            if (title == null || title.Equals(originalResponse?.Title))
                title = null;

            //Author
            var responseAuthor = items.OfType<CellBookAuthor>().Where(p => p.InputType == EBookInputType.Author).First().Item;
            var author = responseAuthor;
            if (author == null || author.Id.Equals(originalResponse?.Author.Id))
                author = null;

            //Genres
            var genres = items.OfType<CellBookGenreItem>().Select(i => i.Genre).ToList();
            if (genres == null || (genres.Count == originalResponse?.Genres.Count && !genres.Except(originalResponse?.Genres).Any()))
                genres = null;

            //Isbn
            long? isbn = items.OfType<CellBookNumberTextView>().Where(p => p.InputType == EBookInputType.ISBN).First().TxtNumberInput;
            if (isbn == null || isbn.ToString().Equals(originalResponse?.ISBN) || isbn == -1)
                isbn = null;

            //Owned
            bool? owned = items.OfType<CellBookSwitch>().Where(p => p.InputType == EBookInputType.Owned).First().IsSelected;
            if (owned == null || owned == originalResponse?.Owned)
                owned = null;

            //Read
            bool? read = items.OfType<CellBookSwitch>().Where(p => p.InputType == EBookInputType.Read).First().IsSelected;
            if (read == null || read == originalResponse?.Read)
                read = null;

            //Pages
            long? pages = items.OfType<CellBookNumberTextView>().Where(p => p.InputType == EBookInputType.Pages).First().TxtNumberInput;
            if (pages == null || pages == originalResponse?.Pages || pages == -1)
                pages = null;

            //Build
            return (new ApiBookDetailRequest
                       .Builder()
                       .SetId(id)
                       .SetTitle(title)
                       .SetAuthor(author?.Id)
                       .SetGenres(genres.GetIdStrings(true))
                       .SetIsbn(isbn)
                       .SetOwned(owned)
                       .SetRead(read)
                       .SetPages(pages)
                       .Build(),
                    responseAuthor,
                    genres);
        }

        #endregion

        #region Public BookDetail

        public static IEnumerable<ICellBook> CreateCellsBookDetail(ApiBookDetailResponse response,
                                                                     Action updateValidation,
                                                                     Action<CellBookGenreItem> removeGenre,
                                                                     Func<Task> deleteBook,
                                                                     IMvxNavigationService navigation,
                                                                     IDevice device)
        {
            var items = new List<ICellBook>
            {
                new CellBookTextView("Title",response.Title, EBookInputType.Title,updateValidation),
                new CellBookAuthor(response.Author,navigation, device, updateValidation),
                new CellBookTitle("Genres")
            };

            foreach (var item in response.Genres)
            {
                items.Add(new CellBookGenreItem(item, removeGenre));
            }

            items.Add(new CellBookNumberTextView("Pages", response.Pages.ToString(), EBookInputType.Pages, updateValidation, false));
            items.Add(new CellBookNumberTextView("ISBN", response.ISBN, EBookInputType.ISBN, updateValidation, false));
            items.Add(new CellBookSwitch("Do you own this book?", response.Owned, EBookInputType.Owned, updateValidation));
            items.Add(new CellBookSwitch("Have you read this book?", response.Read, EBookInputType.Read, updateValidation));
            items.Add(new CellBookButton("Delete Book", deleteBook, false));

            return items;
        }

        #endregion
    }
}