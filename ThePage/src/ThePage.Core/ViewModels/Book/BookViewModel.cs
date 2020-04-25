using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
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

        async Task AddBookWithOLBook(string isbn)
        {
            //Couple to Barcode Scanner in #62
            //test isbn: "9780062286925"
            var olBook = await _openLibraryService.GetBookByISBN(isbn);

            var result = await _navigation.Navigate<AddBookViewModel, AddBookParameter, bool>(new AddBookParameter(isbn, olBook));
            if (result)
                await Refresh();
        }

        #endregion
    }
}
