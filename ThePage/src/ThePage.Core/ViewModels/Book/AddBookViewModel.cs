using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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

    public class AddBookViewModel : BaseViewModel<AddBookParameter, bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        OLObject _olBook;
        string _isbn;

        ApiGenreResponse _genres;
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

            if (result)
            {
                _userInteraction.ToastMessage("Book added");
                await _navigation.Close(this, true);
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
            _genres = await _thePageService.GetAllGenres();

            if (_authors == null || _genres == null)
            {
                _userInteraction.Alert("Error retrieving data from Server", null, "Error");
                await _navigation.Close(this, true);
            }

            if (_olBook != null)
                await CreateCellBooksFromOlData();
            else
                CreateCellBooks();

            IsLoading = false;
            UpdateValidation();
        }

        void CreateCellBooks()
        {
            Items = new MvxObservableCollection<ICellBook>
            {
                new CellBookTextView("Title", EBookInputType.Title,UpdateValidation,true, true),
                new CellBookAuthor(_navigation, _device, UpdateValidation,true),
                new CellBookTitle("Genres"),
                new CellBookAddGenre(AddGenreAction),
                new CellBookNumberTextView("Pages", EBookInputType.Pages, UpdateValidation, true,true),
                new CellBookNumberTextView("ISBN", _isbn, EBookInputType.ISBN, UpdateValidation, false, true),
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
                var result = await _userInteraction.ConfirmAsync($"{olAuthor.Name} is not found in your author list. Would you like to add it?");
                if (!result)
                    return;

                author = new ApiAuthor
                {
                    Name = olAuthor.Name,
                    Olkey = olkey
                };
                var isAdded = await _navigation.Navigate<AddAuthorViewModel, ApiAuthor, bool>(author);

                if (!isAdded)
                    return;

                _authors = await _thePageService.GetAllAuthors();
                author = _authors.Docs.FirstOrDefault(a => a.Olkey != null && a.Olkey.Equals(olkey));
            }

            Items = new MvxObservableCollection<ICellBook>
                {
                    new CellBookTextView("Title",_olBook.Title, EBookInputType.Title,UpdateValidation,true, true),
                    new CellBookAuthor(_navigation, _device, UpdateValidation,true),
                    new CellBookTitle("Genres"),
                    new CellBookAddGenre(AddGenreAction),
                    new CellBookNumberTextView("Pages", _olBook.Pages.ToString(), EBookInputType.Pages, UpdateValidation, true,true),
                    new CellBookNumberTextView("ISBN",_isbn, EBookInputType.ISBN, UpdateValidation, false, true),
                    new CellBookSwitch("Do you own this book?",EBookInputType.Owned, UpdateValidation, true),
                    new CellBookSwitch("Have you read this book?",EBookInputType.Read, UpdateValidation, true),
                    new CellBookButton("Add Book",AddBook)
                };
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.Where(x => x is CellBookInput).OfType<CellBookInput>().ToList();
            var isValid = lstInput.Where(x => x.IsValid == false).Count() == 0;

            var buttons = Items.Where(b => b is CellBookButton).OfType<CellBookButton>();
            buttons.ForEach(x => x.IsValid = isValid);
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