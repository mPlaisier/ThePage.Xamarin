using System;
using System.Threading.Tasks;
using CBP.Extensions;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookViewModel : BaseListViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IOpenLibraryService _openLibraryService;
        readonly IBookService _bookService;

        #region Properties

        public MvxObservableCollection<Book> Books { get; private set; }

        public override string LblTitle => "Books";

        readonly MvxInteraction<GetIsbnCode> _isbnInteraction = new MvxInteraction<GetIsbnCode>();
        public IMvxInteraction<GetIsbnCode> ISBNInteraction => _isbnInteraction;

        #endregion

        #region Constructor

        public BookViewModel(IMvxNavigationService navigation,
                             IOpenLibraryService openLibraryService,
                             IBookService bookService)
        {
            _navigation = navigation;
            _openLibraryService = openLibraryService;
            _bookService = bookService;
        }

        #endregion

        #region Commands

        IMvxAsyncCommand<Book> _itemClickCommand;
        public IMvxAsyncCommand<Book> ItemClickCommand => _itemClickCommand ??= new MvxAsyncCommand<Book>(async (item) =>
        {
            var result = await _navigation.Navigate<BookDetailViewModel, BookDetailParameter, bool>(new BookDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxAsyncCommand _addbookCommand;
        public IMvxAsyncCommand AddBookCommand => _addbookCommand ??= new MvxAsyncCommand(async () =>
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

            await Refresh();
        }

        #endregion

        #region Public

        public override async Task LoadNextPage()
        {
            if (!IsLoading)
            {
                var books = await _bookService.LoadNextBooks();
                Books.AddRange(books);
            }
        }

        public override async Task Search(string input)
        {
            if (IsLoading)
                return;

            var currentSearch = _bookService.SearchText;
            if (currentSearch != null && currentSearch.Equals(input))
                return;

            IsLoading = true;

            var books = await _bookService.Search(input);
            Books = new MvxObservableCollection<Book>(books);

            IsLoading = false;
        }

        public override async Task StopSearch()
        {
            if (_bookService.IsSearching)
                await Refresh().ConfigureAwait(false);
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var books = await _bookService.FetchBooks();
            Books = new MvxObservableCollection<Book>(books);

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