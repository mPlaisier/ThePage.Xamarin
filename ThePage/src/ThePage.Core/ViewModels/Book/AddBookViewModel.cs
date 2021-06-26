using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;
using static ThePage.Core.CellBookInput;

namespace ThePage.Core
{
    public class AddBookParameter
    {
        #region Properties

        public string ISBN { get; }

        public OLObject Book { get; }

        #endregion

        #region Constructor

        public AddBookParameter(string isbn, OLObject book)
        {
            ISBN = isbn;
            Book = book;
        }

        #endregion
    }

    public class AddBookViewModel : BaseViewModel<AddBookParameter, string>
    {
        readonly IMvxNavigationService _navigation;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;
        readonly IGoogleBooksService _googleBooksService;

        readonly IBookService _bookService;
        readonly IAuthorService _authorService;

        OLObject _olBook;
        string _isbn;

        IEnumerable<Author> _authors;

        #region Properties

        public override string LblTitle => "Add book";

        public MvxObservableCollection<ICellBook> Items { get; set; }

        #endregion

        #region Commands

        IMvxAsyncCommand _addbookCommand;
        public IMvxAsyncCommand AddBookCommand => _addbookCommand ??= new MvxAsyncCommand(AddBook);

        #endregion

        #region Constructor

        public AddBookViewModel(IMvxNavigationService navigation,
                                IUserInteraction userInteraction,
                                IDevice device,
                                IGoogleBooksService googleBooksService,
                                IBookService bookService,
                                IAuthorService authorService)
        {
            _navigation = navigation;
            _userInteraction = userInteraction;
            _device = device;
            _googleBooksService = googleBooksService;

            _bookService = bookService;
            _authorService = authorService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(AddBookParameter parameter)
        {
            _isbn = parameter.ISBN;
            _olBook = parameter.Book;
        }

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddBookViewModel)}");

            await base.Initialize();

            await FetchData();
        }

        #endregion

        #region Private

        async Task AddBook()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _bookService.AddBook(Items);
            if (result.IsNotNull())
                await _navigation.Close(this, result.Id);
            else
                IsLoading = false;
        }

        async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            _authors = await _authorService.GetAuthors();

            if (_authors == null)
            {
                _userInteraction.Alert("Error retrieving data from Server", null, "Error");
                await _navigation.Close(this, null);
            }

            if (_olBook != null)
                await CreateCellBooksFromOlData();
            else
                CreateCellBooks(isbn: _isbn);

            IsLoading = false;
            UpdateValidation();
        }

        void CreateCellBooks(string title = null, Author author = null, string pages = null, string isbn = null, ImageLinks images = null)
        {
            Items = new MvxObservableCollection<ICellBook>
                {
                    new CellBasicBook.Builder(title, author, images, UpdateValidation, SearchForBookTitle).IsEdit().Build(),
                    new CellBookTitle("Genres"),
                    new CellBookAddGenre(AddGenreAction),
                    new CellBookNumberTextView.NumberTextViewBuilder("Pages", EBookInputType.Pages,UpdateValidation)
                                              .IsEdit().SetValue(pages).NotRequired()
                                              .Build(),
                    new CellBookNumberTextView.NumberTextViewBuilder("ISBN", EBookInputType.ISBN,UpdateValidation)
                                              .IsEdit().SetValue(isbn).NotRequired().AllowSearch(SearchForBookIsbn)
                                              .Build(),
                    new CellBookSwitch("Do you own this book?",EBookInputType.Owned, UpdateValidation, true),
                    new CellBookSwitch("Have you read this book?",EBookInputType.Read, UpdateValidation, true),
                    new CellBookButton("Add Book", AddBook)
                };
        }

        async Task CreateCellBooksFromOlData()
        {
            var olAuthor = _olBook.Authors.First();
            var olkey = GetAuthorKey(olAuthor?.Url.ToString());

            Author author = null;
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

            CreateCellBooks(_olBook.Title, author, _olBook.Pages.ToString(), _isbn);

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

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.Where(x => x is CellBookInput).OfType<CellBookInput>();
            var isValid = lstInput.All(x => x.IsValid);

            Items.OfType<CellBookButton>().ForEach(x => x.IsValid = isValid);
        }

        async Task AddGenreAction()
        {
            var selectedGenres = Items.Where(g => g is CellBookGenreItem)
                    .OfType<CellBookGenreItem>()
                    .Select(i => i.Genre).ToList();
            var genres = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, List<Genre>>(new SelectedGenreParameters(selectedGenres));

            if (genres != null)
            {
                //Remove all old genres:
                Items.RemoveItems(Items.OfType<CellBookGenreItem>());

                var genreItems = new List<CellBookGenreItem>();
                genres.ForEach(x => genreItems.Add(new CellBookGenreItem(x, RemoveGenre, true)));

                var index = Items.FindIndex(x => x is CellBookAddGenre);
                Items.InsertRange(index, genreItems);
            }
        }

        void RemoveGenre(CellBookGenreItem obj)
        {
            Items.Remove(obj);
        }

        async Task SearchForBookTitle(string title)
        {
            _device.HideKeyboard();
            IsLoading = true;

            var result = await _googleBooksService.SearchBookByTitle(title);
            await HandleSearchResults(result);

            IsLoading = false;
        }

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

                var title = book.VolumeInfo.Title;
                var author = await SelectOrCreateAuthor(new Author(book.VolumeInfo.Authors.First()));

                var pages = book.VolumeInfo.PageCount;
                var isbn = book.VolumeInfo.IndustryIdentifiers.First().Identifier;

                CreateCellBooks(title, author, pages.ToString(), isbn, book.VolumeInfo.ImageLinks);
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

        #endregion
    }
}