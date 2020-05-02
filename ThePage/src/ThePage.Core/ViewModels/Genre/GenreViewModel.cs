using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class GenreViewModel : BaseViewModel, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        public override string Title => "Genres";

        public List<CellGenre> Genres { get; set; }

        #endregion

        #region Constructor

        public GenreViewModel(IMvxNavigationService navigation, IThePageService thePageService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
        }

        #endregion

        #region Commands

        IMvxCommand<CellGenre> _itemClickCommand;
        public IMvxCommand<CellGenre> ItemClickCommand => _itemClickCommand ??= new MvxCommand<CellGenre>(async (item) =>
        {
            var result = await _navigation.Navigate<GenreDetailViewModel, GenreDetailParameter, bool>(new GenreDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxCommand _addGenreCommand;
        public IMvxCommand AddGenreCommand => _addGenreCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddGenreViewModel, bool>();
            if (result)
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

        #endregion

        #region Public

        public async Task Refresh()
        {
            IsLoading = true;

            var genres = await _thePageService.GetAllGenres();
            Genres = GenreBusinessLogic.GenresToCellGenres(genres);

            IsLoading = false;
        }

        #endregion
    }
}
