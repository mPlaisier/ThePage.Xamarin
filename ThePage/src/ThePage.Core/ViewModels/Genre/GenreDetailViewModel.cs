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

        public ApiGenre Genre { get; }

        #endregion

        #region Constructor

        public GenreDetailParameter(ApiGenre genre)
        {
            Genre = genre;
        }

        #endregion
    }

    public class GenreDetailViewModel : BaseViewModel<GenreDetailParameter, bool>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => Genre != null ? Genre.Name : "Genre Detail";

        public ApiGenre Genre { get; internal set; }

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
            if (IsEditing)
                Close();
            else
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
                                    IDevice device)
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

            SetInitialData();
        }

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(GenreDetailViewModel)}");

            return base.Initialize();
        }

        public bool Close()
        {
            if (IsEditing)
            {
                if (HasGenreChanged())
                {
                    _userInteraction.Confirm("Changes have not been saved. Are you sure?",
                                             SetInitialData,
                                             okButton: "Confirm");
                }
                else
                    IsEditing = !IsEditing;
                return true;
            }
            return false;
        }

        #endregion

        #region Private

        async Task UpdateGenre()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;

            if (HasGenreChanged())
            {
                Genre.Name = TxtName;

                var request = new ApiGenreRequest(TxtName);
                var genre = await _thePageService.UpdateGenre(Genre.Id, request);

                if (genre != null)
                {
                    _userInteraction.ToastMessage("Genre updated", EToastType.Success);
                    _updateToolbarInteraction.Raise();
                }
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
                    IsLoading = false;
                }
            }
        }

        bool HasGenreChanged()
        {
            TxtName = TxtName.Trim();
            return !Genre.Name.Equals(TxtName);
        }

        void SetInitialData()
        {
            TxtName = Genre.Name;
            IsEditing = false;
        }

        #endregion
    }
}