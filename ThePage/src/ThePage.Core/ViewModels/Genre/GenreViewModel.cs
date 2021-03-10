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
    public class GenreViewModel : BaseListViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Genres";

        public MvxObservableCollection<ApiGenre> Genres { get; private set; }

        #endregion

        #region Constructor

        public GenreViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region Commands

        IMvxCommand<ApiGenre> _itemClickCommand;
        public IMvxCommand<ApiGenre> ItemClickCommand => _itemClickCommand ??= new MvxCommand<ApiGenre>(async (item) =>
        {
            var result = await _navigation.Navigate<GenreDetailViewModel, GenreDetailParameter, bool>(new GenreDetailParameter(item));
            if (result)
                await Refresh();
        });

        IMvxCommand _addGenreCommand;
        public IMvxCommand AddGenreCommand => _addGenreCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddGenreViewModel, string>();
            if (result != null)
                await Refresh();
        });

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(GenreViewModel)}");

            await base.Initialize();

            Refresh().Forget();
        }

        public override async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiGenreResponse = _isSearching
                   ? await _thePageService.SearchGenres(_search, _currentPage + 1)
                   : await _thePageService.GetNextGenres(_currentPage + 1);
                Genres.AddRange(apiGenreResponse.Docs);

                _currentPage = apiGenreResponse.Page;
                _hasNextPage = apiGenreResponse.HasNextPage;

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

            var apiGenreResponse = await _thePageService.SearchGenres(search);
            Genres = new MvxObservableCollection<ApiGenre>(apiGenreResponse.Docs);

            _currentPage = apiGenreResponse.Page;
            _hasNextPage = apiGenreResponse.HasNextPage;

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

            var result = await _thePageService.GetAllGenres();
            Genres = new MvxObservableCollection<ApiGenre>(result.Docs);

            _currentPage = result.Page;
            _hasNextPage = result.HasNextPage;
            IsLoading = false;
        }

        #endregion
    }
}