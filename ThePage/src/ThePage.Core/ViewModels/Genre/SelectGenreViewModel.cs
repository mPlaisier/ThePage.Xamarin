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

        public List<Genre> Genres { get; internal set; }

        public List<Genre> SelectedGenres { get; internal set; }

        #endregion

        #region Commands

        MvxCommand<Genre> _genreClickCommand;
        public MvxCommand<Genre> GenreClickCommand => _genreClickCommand = _genreClickCommand ?? new MvxCommand<Genre>(HandleGenreClick);

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
            Genres = parameter.Genres;
            SelectedGenres = parameter.SelectedGenres;
        }

        #endregion

        #region Private

        void HandleGenreClick(Genre genre)
        {
            if (SelectedGenres.Contains(genre))
                return;

            _navigation.Close(this, genre);
        }

        async Task Refresh()
        {
            IsLoading = true;

            Genres = await _thePageService.GetAllGenres();

            IsLoading = false;
        }

        #endregion
    }
}
