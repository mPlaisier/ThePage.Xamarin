using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class GenreDetailParameter
    {
        #region Properties

        public CellGenre Genre { get; }

        #endregion

        #region Constructor

        public GenreDetailParameter(CellGenre genre)
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

        public override string Title => "Genre Detail";

        public CellGenre Genre { get; internal set; }

        public string LblName => "Name:";

        string _txtName;
        public string TxtName
        {
            get => _txtName;
            set
            {
                SetProperty(ref _txtName, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblUpdateBtn => "Update Genre";

        public string LblDeleteBtn => "Delete Genre";

        bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

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
            Genre.Name = TxtName;

            var genre = await _thePageService.UpdateGenre(GenreBusinessLogic.CellGenreToGenre(Genre));
            if (genre != null)
                _userInteraction.ToastMessage("Genre updated");
            else
                _userInteraction.Alert("Failure updating genre");

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

                var result = await _thePageService.DeleteGenre(GenreBusinessLogic.CellGenreToGenre(Genre));

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