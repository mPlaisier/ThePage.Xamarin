using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorDetailParameter
    {
        #region Properties

        public AuthorCell Author { get; }

        #endregion

        #region Constructor

        public AuthorDetailParameter(AuthorCell author)
        {
            Author = author;
        }

        #endregion
    }

    public class AuthorDetailViewModel : BaseViewModel<AuthorDetailParameter, bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string Title => "Author Detail";

        public AuthorCell Author { get; internal set; }

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

        public bool IsValid => !string.IsNullOrEmpty(TxtName);

        public string LblUpdateBtn => "Update Author";

        public string LblDeleteBtn => "Delete Author";

        bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        #endregion

        #region Commands

        IMvxCommand _editAuthorCommand;
        public IMvxCommand EditAuthorCommand => _editAuthorCommand ??= new MvxCommand(() =>
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;
        });

        IMvxCommand _deleteAuthorCommand;
        public IMvxCommand DeleteAuthorCommand => _deleteAuthorCommand ??= new MvxCommand(() => DeleteAuthor().Forget());

        IMvxCommand _updateAuthorCommand;
        public IMvxCommand UpdateAuthorCommand => _updateAuthorCommand ??= new MvxCommand(() => UpdateAuthor().Forget());

        #endregion

        #region Constructor

        public AuthorDetailViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(AuthorDetailParameter parameter)
        {
            Author = parameter.Author;

            TxtName = Author.Name;
            IsEditing = false;
        }

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AuthorDetailViewModel)}");

            return base.Initialize();
        }

        #endregion

        #region Private

        async Task UpdateAuthor()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;

            Author.Name = TxtName;

            var author = await _thePageService.UpdateAuthor(AuthorBusinessLogic.AuthorCellToAuthor(Author));
            if (author != null)
                _userInteraction.ToastMessage("Author updated");
            else
                _userInteraction.Alert("Failure updating author");

            IsEditing = false;
            IsLoading = false;
        }

        async Task DeleteAuthor()
        {
            if (IsLoading)
                return;

            if (await _userInteraction.ConfirmAsync("Remove Author?", "Confirm", "DELETE"))
            {
                IsLoading = true;

                var result = await _thePageService.DeleteAuthor(AuthorBusinessLogic.AuthorCellToAuthor(Author));

                if (result)
                {
                    _userInteraction.ToastMessage("Author removed");
                    await _navigation.Close(this, true);
                }
                else
                {
                    _userInteraction.Alert("Failure removing author");
                    IsLoading = false;
                }
            }
        }

        #endregion
    }
}
