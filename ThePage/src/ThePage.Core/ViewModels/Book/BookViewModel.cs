using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        List<BookCell> _books;
        public List<BookCell> Books
        {
            get => _books;
            set => SetProperty(ref _books, value);
        }

        public override string Title => "Books";

        #endregion

        #region Constructor

        public BookViewModel(IMvxNavigationService navigation, IThePageService thePageService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
        }

        #endregion

        #region Commands

        IMvxCommand<BookCell> _itemClickCommand;
        public IMvxCommand<BookCell> ItemClickCommand => _itemClickCommand ??= new MvxCommand<BookCell>(async (item) =>
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

            Books = BookBusinessLogic.BooksToBookCells(books, authors, genres);

            IsLoading = false;
        }

        #endregion
    }
}
