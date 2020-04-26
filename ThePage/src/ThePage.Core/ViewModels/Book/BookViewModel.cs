using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookViewModel : BaseViewModel, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IOpenLibraryService _openLibraryService;

        #region Properties

        public List<CellBook> Books { get; set; }

        public override string Title => "Books";

        MvxInteraction<GetIsbnCode> _isbnInteraction = new MvxInteraction<GetIsbnCode>();
        public IMvxInteraction<GetIsbnCode> ISBNInteraction => _isbnInteraction;

        #endregion

        #region Constructor

        public BookViewModel(IMvxNavigationService navigation, IThePageService thePageService, IOpenLibraryService openLibraryService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _openLibraryService = openLibraryService;
        }

        #endregion

        #region Commands

        IMvxCommand<CellBook> _itemClickCommand;
        public IMvxCommand<CellBook> ItemClickCommand => _itemClickCommand ??= new MvxCommand<CellBook>(async (item) =>
        {
            var result = await _navigation.Navigate<BookDetailViewModel, BookDetailParameter, bool>(new BookDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddBookViewModel, bool>();
            if (result)
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

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var books = await _thePageService.GetAllBooks();
            var authors = await _thePageService.GetAllAuthors();
            var genres = await _thePageService.GetAllGenres();

            Books = BookBusinessLogic.BooksToCellBooks(books, authors, genres);

            IsLoading = false;
        }

        async Task AddBookWithISBN(string isbn)
        {
            IsLoading = true;

            var olBook = await _openLibraryService.GetBookByISBN(isbn);

            var result = await _navigation.Navigate<AddBookViewModel, AddBookParameter, bool>(new AddBookParameter(isbn, olBook));
            if (result)
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
