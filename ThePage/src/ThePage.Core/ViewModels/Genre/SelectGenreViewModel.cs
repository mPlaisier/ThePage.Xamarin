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
        readonly IMvxNavigationService _navigationService;

        #region Properties

        public override string Title => "Select Genre";

        public List<Genre> Genres { get; internal set; }

        public List<Genre> SelectedGenres { get; internal set; }

        #endregion

        #region Commands

        MvxCommand<Genre> _genreClickCommand;
        public MvxCommand<Genre> GenreClickCommand => _genreClickCommand = _genreClickCommand ?? new MvxCommand<Genre>((item) => HandleGenreClick(item).Forget());

        #endregion

        #region Constructor

        public SelectGenreViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(SelectedGenreParameters parameter)
        {
            Genres = parameter.Genres;
            SelectedGenres = parameter.SelectedGenres;
        }

        #endregion

        async Task HandleGenreClick(Genre genre)
        {
            if (SelectedGenres.Contains(genre))
                return;

            await _navigationService.Close(this, genre);
        }
    }
}
