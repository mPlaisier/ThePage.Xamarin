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
    public class BookShelfViewModel : BaseListViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Bookshelves";

        public MvxObservableCollection<ApiBookShelf> BookShelves { get; private set; }

        #endregion

        #region Constructor

        public BookShelfViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region Commands

        IMvxAsyncCommand<ApiBookShelf> _itemClickCommand;
        public IMvxAsyncCommand<ApiBookShelf> ItemClickCommand => _itemClickCommand ??= new MvxAsyncCommand<ApiBookShelf>(GoToBookShelfDetail);

        IMvxAsyncCommand _addBookShelfCommand;
        public IMvxAsyncCommand AddBookShelfCommand => _addBookShelfCommand ??= new MvxAsyncCommand(AddBookShelf);

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookShelfViewModel)}");

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

                var apiBookShelfResponse = _isSearching
                    ? await _thePageService.SearchBookshelves(_search, _currentPage + 1)
                    : await _thePageService.GetNextBookshelves(_currentPage + 1);

                BookShelves.AddRange(apiBookShelfResponse.Docs);

                _currentPage = apiBookShelfResponse.Page;
                _hasNextPage = apiBookShelfResponse.HasNextPage;

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

            var apiBookShelfResponse = await _thePageService.SearchBookshelves(search, null);
            BookShelves = new MvxObservableCollection<ApiBookShelf>(apiBookShelfResponse.Docs);

            _currentPage = apiBookShelfResponse.Page;
            _hasNextPage = apiBookShelfResponse.HasNextPage;

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

            var apiBookShelfResponse = await _thePageService.GetAllBookShelves();
            BookShelves = new MvxObservableCollection<ApiBookShelf>(apiBookShelfResponse.Docs);

            _currentPage = apiBookShelfResponse.Page;
            _hasNextPage = apiBookShelfResponse.HasNextPage;
            IsLoading = false;
        }

        async Task AddBookShelf()
        {
            var result = await _navigation.Navigate<AddBookShelfViewModel, bool>();
            if (result)
                await Refresh();
        }

        async Task GoToBookShelfDetail(ApiBookShelf bookshelf)
        {
            var result = await _navigation.Navigate<BookShelfDetailViewModel, ApiBookShelf, bool>(bookshelf);
            if (result)
                await Refresh();
        }

        #endregion
    }
}