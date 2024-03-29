using System.Collections.Generic;
using System.Linq;
using CBP.Extensions;
using ThePage.Api;
using ThePage.Core.Cells;
using static ThePage.Core.Enums;

namespace ThePage.Core
{
    public static class BookBusinessLogic
    {
        #region Public

        public static (ApiBookDetailRequest request, Author author, IEnumerable<Genre> genres) CreateBookDetailRequestFromInput(IEnumerable<ICellBook> items, string id = null, BookDetail originalResponse = null)
        {
            var builder = new ApiBookDetailRequest.Builder()
                                                   .SetId(id);

            CellBasicBook cellBook = null;
            if (items.Has<ICellBook, CellBasicBook>())
            {
                cellBook = items.OfType<CellBasicBook>()
                                .FirstOrNull(p => p.InputType == EBookInputType.BasicInfo);
            }

            SetTitle();
            var author = SetAuthor();
            var genres = SetGenres();

            SetImages();
            SetIsbn();

            SetOwned();
            SetRead();
            SetPages();

            //Build
            return (builder.Build(),
                    author,
                    genres);

            void SetTitle()
            {
                var title = cellBook.IsNull()
                    ? items.OfType<CellBookTextView>()
                           .First(p => p.InputType == EBookInputType.Title).TxtInput
                    : cellBook.TxtTitle;

                if (!title.Trim().Equals(originalResponse?.Title))
                    builder.SetTitle(title);
            }

            Author SetAuthor()
            {
                var author = cellBook.IsNull()
                    ? items.OfType<CellBookAuthor>().First(p => p.InputType == EBookInputType.Author).Item
                    : cellBook.Author;

                if (author != null && !author.Id.Equals(originalResponse?.Author.Id))
                    builder.SetAuthor(author.Id);

                return author;
            }

            void SetImages()
            {
                var images = cellBook?.Images;
                if (images != null && !images.Equals(originalResponse?.Images))
                    builder.SetImages(ImageBusinessLogic.MapImages(images));
            }

            IEnumerable<Genre> SetGenres()
            {
                var genres = items.OfType<CellBookGenreItem>().Select(i => i.Genre).ToList();
                if (genres != null && (genres.Count != originalResponse?.Genres.Count || genres.Except(originalResponse?.Genres).Any()))
                    builder.SetGenres(genres.GetIdStrings().ToList());

                return genres;
            }

            void SetIsbn()
            {
                long? isbn = items.OfType<CellBookNumberTextView>().First(p => p.InputType == EBookInputType.ISBN).TxtNumberInput;
                if (isbn != null && isbn.HasValue && !isbn.ToString().Equals(originalResponse?.ISBN) && isbn != -1)
                    builder.SetIsbn(isbn.Value);
            }

            void SetOwned()
            {
                bool? owned = items.OfType<CellBookSwitch>().First(p => p.InputType == EBookInputType.Owned).IsSelected;
                if (owned != null && owned.HasValue && owned != originalResponse?.Owned)
                    builder.SetOwned(owned.Value);
            }

            void SetRead()
            {
                bool? read = items.OfType<CellBookSwitch>().First(p => p.InputType == EBookInputType.Read).IsSelected;
                if (read != null && read.HasValue && read != originalResponse?.Read)
                    builder.SetRead(read.Value);
            }

            void SetPages()
            {
                long? pages = items.OfType<CellBookNumberTextView>().First(p => p.InputType == EBookInputType.Pages).TxtNumberInput;
                if (pages != null && pages.HasValue && pages != originalResponse?.Pages && pages != -1)
                    builder.SetPages(pages.Value);
            }
        }

        public static IEnumerable<Book> MapBooks(IEnumerable<ApiBook> apiBooks)
        {
            return apiBooks.Select(book => MapBook(book));
        }

        public static Book MapBook(ApiBook book)
        {
            return new Book()
            {
                Id = book.Id,
                Title = book.Title,
                Author = AuthorBusinessLogic.MapAuthor(book.Author),
                Images = ImageBusinessLogic.MapImages(book.Images)
            };
        }

        public static Book MapBook(BookDetail book)
        {
            return new Book()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Images = book.Images
            };
        }

        public static BookDetail MapBookDetail(ApiBookDetailResponse bookDetail)
        {
            return new BookDetail()
            {
                Id = bookDetail.Id,
                Title = bookDetail.Title,
                Author = AuthorBusinessLogic.MapAuthor(bookDetail.Author),
                Genres = GenreBusinessLogic.MapGenres(bookDetail.Genres).ToList(),
                ISBN = bookDetail.ISBN,
                Owned = bookDetail.Owned,
                Read = bookDetail.Read,
                Pages = bookDetail.Pages,
                Ebook = bookDetail.Ebook,
                Images = ImageBusinessLogic.MapImages(bookDetail.Images)
            };
        }

        #endregion
    }
}