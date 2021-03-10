using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorViewModel : BaseListViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Authors";

        public MvxObservableCollection<ApiAuthor> Authors { get; private set; }

        #endregion

        #region Constructor

        public AuthorViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region Commands

        IMvxCommand<ApiAuthor> _itemClickCommand;
        public IMvxCommand<ApiAuthor> ItemClickCommand => _itemClickCommand ??= new MvxCommand<ApiAuthor>(async (item) =>
        {
            var result = await _navigation.Navigate<AuthorDetailViewModel, AuthorDetailParameter, bool>(new AuthorDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxCommand _addAuthorCommand;
        public IMvxCommand AddAuthorCommand => _addAuthorCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddAuthorViewModel, ApiAuthor>();
            if (result != null)
                await Refresh();
        });

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AuthorViewModel)}");

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

                var apiAuthorResponse = _isSearching
                    ? await _thePageService.SearchAuthors(_search, _currentPage + 1)
                    : await _thePageService.GetNextAuthors(_currentPage + 1);

                Authors.AddRange(apiAuthorResponse.Docs);

                _currentPage = apiAuthorResponse.Page;
                _hasNextPage = apiAuthorResponse.HasNextPage;

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

            var apiAuthorResponse = await _thePageService.SearchAuthors(search);
            Authors = new MvxObservableCollection<ApiAuthor>(apiAuthorResponse.Docs);

            _currentPage = apiAuthorResponse.Page;
            _hasNextPage = apiAuthorResponse.HasNextPage;

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

            var authors = await _thePageService.GetAllAuthors();
            Authors = new MvxObservableCollection<ApiAuthor>(authors.Docs);

            _currentPage = authors.Page;
            _hasNextPage = authors.HasNextPage;
            IsLoading = false;
        }

        #endregion
    }
}