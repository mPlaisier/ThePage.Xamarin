using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class SelectedGenreParameters
    {
        #region Properties

        public List<Genre> Genres { get; }

        public List<Genre> SelectedGenres { get; }

        #endregion

        #region Constructor

        public SelectedGenreParameters(List<Genre> genres, List<Genre> selectedGenres)
        {
            Genres = genres;
            SelectedGenres = selectedGenres;
        }

        #endregion
    }

    public class SelectGenreViewModel : BaseViewModel<SelectedGenreParameters, Genre>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        public override string Title => "Select Genre";

        public List<CellGenre> CellGenres { get; internal set; }

        public List<Genre> SelectedGenres { get; internal set; }

        #endregion

        #region Commands

        MvxCommand<CellGenre> _genreClickCommand;
        public MvxCommand<CellGenre> GenreClickCommand => _genreClickCommand = _genreClickCommand ?? new MvxCommand<CellGenre>(HandleGenreClick);

        IMvxCommand _addGenreCommand;
        public IMvxCommand AddGenreCommand => _addGenreCommand ??= new MvxCommand(async () =>
       {
           var result = await _navigation.Navigate<AddGenreViewModel, bool>();
           if (result)
               await Refresh();

       });

        #endregion

        #region Constructor

        public SelectGenreViewModel(IMvxNavigationService navigationService, IThePageService thePageService)
        {
            _navigation = navigationService;
            _thePageService = thePageService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(SelectedGenreParameters parameter)
        {
            CellGenres = GenreBusinessLogic.GenresToCellGenres(parameter.Genres);
            SelectedGenres = parameter.SelectedGenres;
        }

        #endregion

        #region Private

        void HandleGenreClick(CellGenre genre)
        {
            if (SelectedGenres.Contains(genre.Genre))
                return;

            _navigation.Close(this, genre.Genre);
        }

        async Task Refresh()
        {
            IsLoading = true;

            CellGenres = GenreBusinessLogic.GenresToCellGenres(await _thePageService.GetAllGenres());

            IsLoading = false;
        }

        #endregion
    }
}
