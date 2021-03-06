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
            var builder = new ApiBookDetailRequest.Builder()
                                                   .SetId(id);

            //Title
            var title = items.OfType<CellBookTextView>().First(p => p.InputType == EBookInputType.Title).TxtInput.Trim();
            if (title != null && !title.Equals(originalResponse?.Title))
                builder.SetTitle(title);

            //Author
            var responseAuthor = items.OfType<CellBookAuthor>().First(p => p.InputType == EBookInputType.Author).Item;
            var author = responseAuthor;
            if (author != null && !author.Id.Equals(originalResponse?.Author.Id))
                builder.SetAuthor(author.Id);

            //Genres
            var genres = items.OfType<CellBookGenreItem>().Select(i => i.Genre).ToList();
            if (genres != null && (genres.Count != originalResponse?.Genres.Count || genres.Except(originalResponse?.Genres).Any()))
                builder.SetGenres(genres.GetIdStrings().ToList());

            //Isbn
            long? isbn = items.OfType<CellBookNumberTextView>().First(p => p.InputType == EBookInputType.ISBN).TxtNumberInput;
            if (isbn != null && isbn.HasValue && !isbn.ToString().Equals(originalResponse?.ISBN) && isbn != -1)
                builder.SetIsbn(isbn.Value);

            //Owned
            bool? owned = items.OfType<CellBookSwitch>().First(p => p.InputType == EBookInputType.Owned).IsSelected;
            if (owned != null && owned.HasValue && owned != originalResponse?.Owned)
                builder.SetOwned(owned.Value);

            //Read
            bool? read = items.OfType<CellBookSwitch>().First(p => p.InputType == EBookInputType.Read).IsSelected;
            if (read != null && read.HasValue && read != originalResponse?.Read)
                builder.SetRead(read.Value);

            //Pages
            long? pages = items.OfType<CellBookNumberTextView>().First(p => p.InputType == EBookInputType.Pages).TxtNumberInput;
            if (pages != null && pages.HasValue && pages != originalResponse?.Pages && pages != -1)
                builder.SetPages(pages.Value);

            //Build
            return (builder.Build(),
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