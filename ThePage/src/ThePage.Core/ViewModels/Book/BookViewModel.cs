using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookViewModel : BaseListViewModel, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IOpenLibraryService _openLibraryService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public MvxObservableCollection<ApiBook> Books { get; private set; }

        public override string LblTitle => "Books";

        MvxInteraction<GetIsbnCode> _isbnInteraction = new MvxInteraction<GetIsbnCode>();
        public IMvxInteraction<GetIsbnCode> ISBNInteraction => _isbnInteraction;

        #endregion

        #region Constructor

        public BookViewModel(IMvxNavigationService navigation,
                             IThePageService thePageService,
                             IOpenLibraryService openLibraryService,
                             IUserInteraction userInteraction,
                             IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _openLibraryService = openLibraryService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region Commands

        IMvxCommand<ApiBook> _itemClickCommand;
        public IMvxCommand<ApiBook> ItemClickCommand => _itemClickCommand ??= new MvxCommand<ApiBook>(async (item) =>
        {
            var result = await _navigation.Navigate<BookDetailViewModel, BookDetailParameter, bool>(new BookDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddBookViewModel, string>();
            if (result != null)
                await Refresh();
        });

        IMvxCommand _scanBarcodeCommand;
        public IMvxCommand ScanBarcodeCommand => _scanBarcodeCommand ??= new MvxCommand(StartBarcodeScanner);

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookViewModel)}");

            await base.Initialize();

            Refresh().Forget();
        }

        #endregion

        #region Public

        public override async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiBookResponse = _isSearching
                    ? await _thePageService.SearchBooksTitle(_search, _currentPage + 1)
                    : await _thePageService.GetNextBooks(_currentPage + 1);

                Books.AddRange(apiBookResponse.Docs);

                _currentPage = apiBookResponse.Page;
                _hasNextPage = apiBookResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
            }
        }

        public override async Task Search(string search)
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();

            if (_search != null && _search.Equals(search))
                return;

            IsLoading = true;
            _search = search;
            _isSearching = true;

            var apiBookResponse = await _thePageService.SearchBooksTitle(search, null);
            Books = new MvxObservableCollection<ApiBook>(apiBookResponse.Docs);

            _currentPage = apiBookResponse.Page;
            _hasNextPage = apiBookResponse.HasNextPage;

            IsLoading = false;
        }

        public override void StopSearch()
        {
            if (_isSearching)
            {
                _isSearching = false;
                _search = null;
                Refresh().Forget();
            }
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var apiBookResponse = await _thePageService.GetAllBooks();
            Books = new MvxObservableCollection<ApiBook>(apiBookResponse.Docs);
            _currentPage = apiBookResponse.Page;
            _hasNextPage = apiBookResponse.HasNextPage;

            IsLoading = false;
        }

        async Task AddBookWithISBN(string isbn)
        {
            IsLoading = true;

            var olBook = await _openLibraryService.GetBookByISBN(isbn);

            var result = await _navigation.Navigate<AddBookViewModel, AddBookParameter, string>(new AddBookParameter(isbn, olBook));
            if (result != null)
                await Refresh();
            else
                IsLoading = false;
        }

        void StartBarcodeScanner()
        {
            var request = new GetIsbnCode
            {
                ISBNCallback = (isbn) =>
                {
                    if (isbn != null && isbn != string.Empty)
                        AddBookWithISBN(isbn).Forget();
                }
            };

            _isbnInteraction.Raise(request);
        }

        #endregion

        public class GetIsbnCode
        {
            public Action<string> ISBNCallback { get; set; }
        }
    }
}