using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class GenreViewModel : BaseListViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IGenreService _genreService;

        #region Properties

        public override string LblTitle => "Genres";

        public MvxObservableCollection<Genre> Genres { get; private set; }

        #endregion

        #region Constructor

        public GenreViewModel(IMvxNavigationService navigation, IGenreService genreService)
        {
            _navigation = navigation;
            _genreService = genreService;
        }

        #endregion

        #region Commands

        IMvxCommand<Genre> _itemClickCommand;
        public IMvxCommand<Genre> ItemClickCommand => _itemClickCommand ??= new MvxCommand<Genre>(async (item) =>
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

            await Refresh();
        }

        public override async Task LoadNextPage()
        {
            if (!IsLoading)
            {
                var genres = await _genreService.LoadNextGenres();
                Genres.AddRange(genres);
            }
        }

        public override async Task Search(string input)
        {
            if (IsLoading)
                return;

            var currentSearch = _genreService.SearchText;
            if (currentSearch != null && currentSearch.Equals(input))
                return;

            IsLoading = true;

            var genres = await _genreService.Search(input);
            Genres = new MvxObservableCollection<Genre>(genres);

            IsLoading = false;
        }

        public override async Task StopSearch()
        {
            if (_genreService.IsSearching)
                await Refresh().ConfigureAwait(false);
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var genres = await _genreService.GetGenres();
            Genres = new MvxObservableCollection<Genre>(genres);

            IsLoading = false;
        }

        #endregion
    }
}