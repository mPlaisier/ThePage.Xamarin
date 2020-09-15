using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using PropertyChanged;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class GenreDetailParameter
    {
        #region Properties

        public ApiGenre Genre { get; }

        #endregion

        #region Constructor

        public GenreDetailParameter(ApiGenre genre)
        {
            Genre = genre;
        }

        #endregion
    }

    public class GenreDetailViewModel : BaseViewModel<GenreDetailParameter, bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Genre Detail";

        public ApiGenre Genre { get; internal set; }

        public string LblName => "Name:";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtName { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblUpdateBtn => "Update Genre";

        public string LblDeleteBtn => "Delete Genre";

        public bool IsEditing { get; set; }

        #endregion

        #region Commands

        IMvxCommand _editGenreCommand;
        public IMvxCommand EditGenreCommand => _editGenreCommand ??= new MvxCommand(() =>
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;
        });

        IMvxCommand _deleteGenreCommand;
        public IMvxCommand DeleteGenreCommand => _deleteGenreCommand ??= new MvxCommand(() => DeleteGenre().Forget());

        IMvxCommand _updateGenreCommand;
        public IMvxCommand UpdateGenreCommand => _updateGenreCommand ??= new MvxCommand(() => UpdateGenre().Forget());

        #endregion

        #region Constructor

        public GenreDetailViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
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
                    _userInteraction.ToastMessage("Genre updated", EToastType.Success);
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

                var result = await _thePageService.DeleteGenre(Genre);

                if (result)
                {
                    _userInteraction.ToastMessage("Genre removed");
                    await _navigation.Close(this, true);
                }
                else
                {
                    _userInteraction.Alert("Failure removing genre");
                    IsLoading = false;
                }
            }
        }

        #endregion
    }
}