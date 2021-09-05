using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PropertyChanged;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class GenreDetailParameter
    {
        #region Properties

        public Genre Genre { get; }

        #endregion

        #region Constructor

        public GenreDetailParameter(Genre genre)
        {
            Genre = genre;
        }

        #endregion
    }

    public class GenreDetailViewModel : BaseViewModel<GenreDetailParameter, bool>
    {
        readonly IMvxNavigationService _navigation;
        readonly IDevice _device;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IGenreService _genreService;

        #region Properties

        public override string LblTitle => Genre != null ? Genre.Name : "Genre Detail";

        public Genre Genre { get; internal set; }

        public string LblName => "Name:";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtName { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblUpdateBtn => "Update Genre";

        public string LblDeleteBtn => "Delete Genre";

        public bool IsEditing { get; set; }

        readonly MvxInteraction _updateToolbarInteraction = new MvxInteraction();
        public IMvxInteraction UpdateToolbarInteraction => _updateToolbarInteraction;

        #endregion

        #region Commands

        IMvxCommand _editGenreCommand;
        public IMvxCommand EditGenreCommand => _editGenreCommand ??= new MvxCommand(() =>
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;
        });

        IMvxAsyncCommand _deleteGenreCommand;
        public IMvxAsyncCommand DeleteGenreCommand => _deleteGenreCommand ??= new MvxAsyncCommand(DeleteGenre);

        IMvxAsyncCommand _updateGenreCommand;
        public IMvxAsyncCommand UpdateGenreCommand => _updateGenreCommand ??= new MvxAsyncCommand(UpdateGenre);

        #endregion

        #region Constructor

        public GenreDetailViewModel(IMvxNavigationService navigation,
                                    IThePageService thePageService,
                                    IUserInteraction userInteraction,
                                    IDevice device,
                                    IGenreService genreService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;

            _genreService = genreService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(GenreDetailParameter parameter)
        {
            Genre = parameter.Genre;

            TxtName = Genre.Name;
            IsEditing = false;
        }

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(GenreDetailViewModel)}");

            return base.Initialize();
        }

        #endregion

        #region Private

        async Task UpdateGenre()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;

            TxtName = TxtName.Trim();
            if (!Genre.Name.Equals(TxtName))
            {
                Genre.Name = TxtName;

                var request = new ApiGenreRequest(TxtName);
                var genre = await _thePageService.UpdateGenre(Genre.Id, request);

                if (genre != null)
                {
                    _userInteraction.ToastMessage("Genre updated", EToastType.Success);

                    _updateToolbarInteraction.Raise();
                }
                else
                    _userInteraction.Alert("Failure updating genre");
            }

            IsEditing = false;
            IsLoading = false;
        }

        async Task DeleteGenre()
        {
            if (IsLoading)
                return;

            if (await _userInteraction.ConfirmAsync("Remove Genre?", "Confirm", "DELETE"))
            {
                IsLoading = true;

                var result = await _genreService.DeleteGenre(Genre.Id);

                if (result)
                    await _navigation.Close(this, true);
                else
                    IsLoading = false;
            }
        }

        #endregion
    }
}