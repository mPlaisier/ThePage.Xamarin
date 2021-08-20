using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.Cells;
using static ThePage.Core.Enums;

namespace ThePage.Core
{
    [ThePageTypeService]
    public class AddBookScreenManagerService : BaseBookDetailScreenManager, IAddBookScreenManagerService
    {
        readonly IGoogleBooksService _googleBooksService;

        readonly IBookService _bookService;
        readonly IAuthorService _authorService;

        Action<string> _actionClose;

        OLObject _olBook;
        string _isbn;

        #region Constructor

        public AddBookScreenManagerService(IMvxNavigationService navigationService,
                                           IUserInteraction userInteraction,
                                           IDevice device,
                                           IGoogleBooksService googleBooksService,
                                           IBookService bookService,
                                           IAuthorService authorService)
            : base(navigationService, userInteraction, device)
        {
            _googleBooksService = googleBooksService;

            _bookService = bookService;
            _authorService = authorService;
        }

        #endregion

        #region Public

        public void Init(AddBookParameter parameter, Action<string> actionClose)
        {
            _isbn = parameter?.ISBN;
            _olBook = parameter?.Book;

            _actionClose = actionClose;
        }

        public override async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            if (_olBook != null)
                await CreateCellBooksFromOlData();
            else
                CreateCellBooks(new BookDetail { ISBN = _isbn });

            IsLoading = false;
            UpdateValidation();
        }

        public override async Task SaveBook()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _bookService.AddBook(Items);
            if (result.IsNotNull())
                _actionClose?.Invoke(result.Id);
            else
                IsLoading = false;
        }

        public override void CreateCellBooks(BookDetail bookDetail)
        {
            Items = new MvxObservableCollection<ICellBook>
            {
                new CellBasicBook.Builder(bookDetail.Title, bookDetail.Author, bookDetail.Images, UpdateValidation, SearchForBookTitle, SelectAuthor).IsEdit().Build(),
                new CellBookTitle("Genres"),
                new CellBookAddGenre(AddGenre),
                new CellBookNumberTextView.NumberTextViewBuilder("Pages", EBookInputType.Pages,UpdateValidation)
                                            .IsEdit().SetValue(bookDetail.Pages.ToString()).NotRequired()
                                            .Build(),
                new CellBookNumberTextView.NumberTextViewBuilder("ISBN", EBookInputType.ISBN,UpdateValidation)
                                            .IsEdit().SetValue(bookDetail.ISBN).NotRequired().AllowSearch(SearchForBookIsbn)
                                            .Build(),
                new CellBookSwitch("Do you own this book?",EBookInputType.Owned, UpdateValidation, true),
                new CellBookSwitch("Have you read this book?",EBookInputType.Read, UpdateValidation, true),
                new CellBookButton("Add Book", SaveBook)
            };
        }

        #endregion

        #region Private

        async Task SearchForBookIsbn(string isbn)
        {
            _device.HideKeyboard();
            IsLoading = true;

            var result = await _googleBooksService.SearchBookByISBN(isbn);
            await HandleSearchResults(result);

            IsLoading = false;
        }

        async Task HandleSearchResults(GoogleBooksResult result)
        {
            if (result == null)
                return;

            if (result.Books.IsNotNullAndHasItems())
            {
                var book = await _navigation.Navigate<BookSearchViewModel, GoogleBooksResult, GoogleBook>(result);

                if (book != null)
                {
                    var title = book.VolumeInfo.Title;
                    var author = await SelectOrCreateAuthor(new Author(book.VolumeInfo.Authors.First()));

                    var pages = book.VolumeInfo.PageCount;
                    var isbn = book.VolumeInfo.IndustryIdentifiers.First().Identifier;

                    var bookDetail = new BookDetail
                    {
                        Title = title,
                        Author = author,
                        Pages = pages,
                        ISBN = isbn,
                        Images = BookBusinessLogic.MapImageLinksToCore(book.VolumeInfo.ImageLinks)
                    };

                    CreateCellBooks(bookDetail);
                    UpdateValidation();
                }
            }
            else
            {
                _userInteraction.Alert("No books found");
            }
        }

        async Task<Author> SelectOrCreateAuthor(Author author, string olKey = null)
        {
            var userChoice = await _userInteraction.ConfirmThreeButtonsAsync($"{author.Name} is not found in your author list. Would you like to add it?",
                                                                             neutral: "Choose from list");

            Author newAuthor = null;
            if (userChoice == ConfirmThreeButtonsResponse.Positive)
            {
                newAuthor = await _navigation.Navigate<AddAuthorViewModel, Author, Author>(author);
            }
            //Select author from list
            else if (userChoice == ConfirmThreeButtonsResponse.Neutral)
            {
                newAuthor = await _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, Author>(new AuthorSelectParameter());

                if (olKey.IsNotNull())
                {
                    newAuthor.Olkey = olKey;
                    newAuthor = await _authorService.UpdateAuthor(newAuthor);
                }
            }

            return newAuthor;
        }

        async Task SearchForBookTitle(string title)
        {
            _device.HideKeyboard();
            IsLoading = true;

            var result = await _googleBooksService.SearchBookByTitle(title);
            await HandleSearchResults(result);

            IsLoading = false;
        }

        async Task CreateCellBooksFromOlData()
        {
            var olAuthor = _olBook.Authors.First();
            var olkey = GetAuthorKey(olAuthor?.Url.ToString());

            Author author = null;

            var _authors = await _authorService.GetAuthors();
            if (_authors == null)
            {
                _userInteraction.Alert("Error retrieving data from Server", null, "Error");
                _actionClose?.Invoke(null);
            }
            else
            {
                author = _authors.FirstOrDefault(a => a.Olkey != null && a.Olkey.Equals(olkey));

                if (author == null)
                {
                    author = new Author
                    {
                        Name = olAuthor?.Name,
                        Olkey = olkey
                    };
                    var newAuthor = await SelectOrCreateAuthor(author, olkey);

                    if (newAuthor != null)
                        author = newAuthor;
                    else
                    {
                        _authors = await _authorService.GetAuthors();
                        author = _authors.FirstOrNull(a => a.Olkey != null && a.Olkey.Equals(olkey));
                    }
                }

                var bookDetail = new BookDetail
                {
                    Title = _olBook.Title,
                    Author = author,
                    Pages = _olBook.Pages,
                    ISBN = _isbn
                };

                CreateCellBooks(bookDetail);
            }

            static string GetAuthorKey(string key)
            {
                if (key != null)
                {
                    var split = key.Split('/');
                    return split[4];
                }
                return string.Empty;
            }
        }

        Task<Author> SelectAuthor(Author author)
        {
            _device.HideKeyboard();

            return _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, Author>(new AuthorSelectParameter(author));
        }

        #endregion
    }
}
