using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookShelfViewModel : BaseListViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IBookShelfService _bookshelfService;

        #region Properties

        public override string LblTitle => "Bookshelves";

        public MvxObservableCollection<Bookshelf> BookShelves { get; private set; }

        #endregion

        #region Constructor

        public BookShelfViewModel(IMvxNavigationService navigation, IBookShelfService bookshelfService)
        {
            _navigation = navigation;
            _bookshelfService = bookshelfService;
        }

        #endregion

        #region Commands

        IMvxAsyncCommand<Bookshelf> _itemClickCommand;
        public IMvxAsyncCommand<Bookshelf> ItemClickCommand => _itemClickCommand ??= new MvxAsyncCommand<Bookshelf>(GoToBookShelfDetail);

        IMvxAsyncCommand _addBookShelfCommand;
        public IMvxAsyncCommand AddBookShelfCommand => _addBookShelfCommand ??= new MvxAsyncCommand(AddBookShelf);

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookShelfViewModel)}");

            await base.Initialize();

            await Refresh();
        }

        #endregion

        #region Public

        public override async Task LoadNextPage()
        {
            if (!IsLoading)
            {
                var bookshelves = await _bookshelfService.LoadNextBookshelves();
                BookShelves.AddRange(bookshelves);
            }
        }

        public override async Task Search(string input)
        {
            if (IsLoading)
                return;

            var currentSearch = _bookshelfService.SearchText;
            if (currentSearch != null && currentSearch.Equals(input))
                return;

            IsLoading = true;

            var bookshelves = await _bookshelfService.Search(input);
            BookShelves = new MvxObservableCollection<Bookshelf>(bookshelves);

            IsLoading = false;
        }

        public override async Task StopSearch()
        {
            if (_bookshelfService.IsSearching)
                await Refresh().ConfigureAwait(false);
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var bookshelf = await _bookshelfService.FetchBookshelves();
            BookShelves = new MvxObservableCollection<Bookshelf>(bookshelf);

            IsLoading = false;
        }

        async Task AddBookShelf()
        {
            var result = await _navigation.Navigate<AddBookShelfViewModel, bool>();
            if (result)
                await Refresh();
        }

        async Task GoToBookShelfDetail(Bookshelf bookshelf)
        {
            var result = await _navigation.Navigate<BookShelfDetailViewModel, Bookshelf, bool>(bookshelf);
            if (result)
                await Refresh();
        }

        #endregion
    }
}