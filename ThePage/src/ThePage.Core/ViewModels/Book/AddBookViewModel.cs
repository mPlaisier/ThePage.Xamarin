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
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        OLObject _olBook;
        string _isbn;

        ApiAuthorResponse _authors;

        #region Properties

        public override string LblTitle => "Add book";

        public MvxObservableCollection<ICellBook> Items { get; set; }

        #endregion

        #region Commands

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(() => AddBook().Forget());

        #endregion

        #region Constructor

        public AddBookViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
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

            FetchData().Forget();
        }

        #endregion

        #region Private

        async Task AddBook()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            _device.HideKeyboard();

            var (book, author, genres) = BookBusinessLogic.CreateBookDetailRequestFromInput(Items);
            var result = await _thePageService.AddBook(book);

            if (result != null)
            {
                _userInteraction.ToastMessage("Book added");
                await _navigation.Close(this, result.Id);
            }
            else
            {
                _userInteraction.Alert("Failure adding book");
                IsLoading = false;
            }
        }

        async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            _authors = await _thePageService.GetAllAuthors();
            var _genres = await _thePageService.GetAllGenres();

            if (_authors == null || _genres == null)
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

        void CreateCellBooks(string title = null, ApiAuthor author = null, string pages = null, string isbn = null)
        {
            Items = new MvxObservableCollection<ICellBook>
                {
                    new CellBookTextView("Title",title, EBookInputType.Title,UpdateValidation,true, true),
                    new CellBookAuthor(author, _navigation, _device, UpdateValidation,true),
                    new CellBookTitle("Genres"),
                    new CellBookAddGenre(AddGenreAction),
                    new CellBookNumberTextView("Pages", pages, EBookInputType.Pages, UpdateValidation, false,true),
                    new CellBookNumberTextView("ISBN",isbn, EBookInputType.ISBN, UpdateValidation, false, true),
                    new CellBookSwitch("Do you own this book?",EBookInputType.Owned, UpdateValidation, true),
                    new CellBookSwitch("Have you read this book?",EBookInputType.Read, UpdateValidation, true),
                    new CellBookButton("Add Book",AddBook)
                };
        }

        async Task CreateCellBooksFromOlData()
        {
            var olAuthor = _olBook.Authors.First();
            var olkey = GetAuthorKey(olAuthor?.Url.ToString());

            ApiAuthor author = null;
            author = _authors.Docs.FirstOrDefault(a => a.Olkey != null && a.Olkey.Equals(olkey));

            if (author == null)
            {
                var authorChoice = await _userInteraction.ConfirmThreeButtonsAsync($"{olAuthor?.Name} is not found in your author list. Would you like to add it?", null,
                                                                                   neutral: "Choose from list");

                ApiAuthor newAuthor = null;
                if (authorChoice == ConfirmThreeButtonsResponse.Positive)
                {
                    author = new ApiAuthor
                    {
                        Name = olAuthor?.Name,
                        Olkey = olkey
                    };
                    newAuthor = await _navigation.Navigate<AddAuthorViewModel, ApiAuthor, ApiAuthor>(author);
                }
                //Select author from list
                else if (authorChoice == ConfirmThreeButtonsResponse.Neutral)
                {
                    newAuthor = await _navigation.Navigate<AuthorSelectViewModel, AuthorSelectParameter, ApiAuthor>(new AuthorSelectParameter(null));
                    newAuthor.Olkey = olkey;

                    newAuthor = await _thePageService.UpdateAuthor(newAuthor.Id, new ApiAuthorRequest(newAuthor));
                }

                if (newAuthor != null)
                    author = newAuthor;
                else
                {
                    _authors = await _thePageService.GetAllAuthors();
                    author = _authors.Docs.FirstOrDefault(a => a.Olkey != null && a.Olkey.Equals(olkey));
                }
            }

            CreateCellBooks(_olBook.Title, author, _olBook.Pages.ToString(), _isbn);
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.Where(x => x is CellBookInput).OfType<CellBookInput>().ToList();
            var isValid = lstInput.All(x => x.IsValid);

            Items.OfType<CellBookButton>().ForEach(x => x.IsValid = isValid);
        }

        async Task AddGenreAction()
        {
            var selectedGenres = Items.Where(g => g is CellBookGenreItem)
                    .OfType<CellBookGenreItem>()
                    .Select(i => i.Genre).ToList();
            var genres = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, List<ApiGenre>>(new SelectedGenreParameters(selectedGenres));

            if (genres != null)
            {
                //Remove all old genres:
                Items.RemoveItems(Items.OfType<CellBookGenreItem>().ToList());

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

        string GetAuthorKey(string key)
        {
            if (key != null)
            {
                var split = key.Split('/');
                return split[4];
            }
            return string.Empty;
        }

        #endregion
    }
}