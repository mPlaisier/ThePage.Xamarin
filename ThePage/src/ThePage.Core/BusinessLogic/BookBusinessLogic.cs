using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using Nelibur.ObjectMapper;
using ThePage.Api;
using static ThePage.Core.CellBookInput;

namespace ThePage.Core
{
    public static class BookBusinessLogic
    {
        #region Public

        public static (ApiBookDetailRequest request, Author author, IEnumerable<Genre> genres) CreateBookDetailRequestFromInput(IEnumerable<ICellBook> items, string id = null, BookDetail originalResponse = null)
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

        public static IEnumerable<Book> MapBooks(IEnumerable<ApiBook> apiBooks)
        {
            return apiBooks.Select(book => MapBook(book));
        }

        public static Book MapBook(ApiBook book)
        {
            TinyMapper.Bind<ApiBook, Book>();
            TinyMapper.Bind<ApiAuthor, Author>();
            return TinyMapper.Map<Book>(book);
        }

        public static Book MapBook(BookDetail book)
        {
            TinyMapper.Bind<ApiBook, Book>();
            TinyMapper.Bind<ApiAuthor, Author>();
            return TinyMapper.Map<Book>(book);
        }

        public static BookDetail MapBookDetail(ApiBookDetailResponse bookDetail)
        {
            TinyMapper.Bind<ApiBookDetailResponse, BookDetail>();
            TinyMapper.Bind<ApiAuthor, Author>();
            TinyMapper.Bind<ApiGenre, Genre>();
            return TinyMapper.Map<BookDetail>(bookDetail);
        }

        #endregion

        #region Public BookDetail

        public static IEnumerable<ICellBook> CreateCellsBookDetail(BookDetail bookDetail,
                                                                     Action updateValidation,
                                                                     Action<CellBookGenreItem> removeGenre,
                                                                     Func<Task> deleteBook,
                                                                     IMvxNavigationService navigation,
                                                                     IDevice device)
        {
            var items = new List<ICellBook>
            {
                new CellBookTextView("Title",bookDetail.Title, EBookInputType.Title,updateValidation),
                new CellBookAuthor(bookDetail.Author,navigation, device, updateValidation),
                new CellBookTitle("Genres")
            };

            foreach (var item in bookDetail?.Genres)
            {
                items.Add(new CellBookGenreItem(item, removeGenre));
            }

            items.Add(new CellBookNumberTextView("Pages", bookDetail.Pages.ToString(), EBookInputType.Pages, updateValidation, false));
            items.Add(new CellBookNumberTextView("ISBN", bookDetail.ISBN, EBookInputType.ISBN, updateValidation, false));
            items.Add(new CellBookSwitch("Do you own this book?", bookDetail.Owned, EBookInputType.Owned, updateValidation));
            items.Add(new CellBookSwitch("Have you read this book?", bookDetail.Read, EBookInputType.Read, updateValidation));
            items.Add(new CellBookButton("Delete Book", deleteBook, false));

            return items;
        }

        #endregion
    }
}