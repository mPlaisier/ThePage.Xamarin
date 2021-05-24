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
    public class AuthorDetailParameter
    {
        #region Properties

        public ApiAuthor Author { get; }

        #endregion

        #region Constructor

        public AuthorDetailParameter(ApiAuthor author)
        {
            Author = author;
        }

        #endregion
    }

    public class AuthorDetailViewModel : BaseViewModel<AuthorDetailParameter, bool>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => Author != null ? Author.Name : "Author Detail";

        public ApiAuthor Author { get; internal set; }

        public string LblName => "Name:";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtName { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblUpdateBtn => "Update Author";

        public string LblDeleteBtn => "Delete Author";

        public bool IsEditing { get; set; }

        readonly MvxInteraction _updateToolbarInteraction = new MvxInteraction();
        public IMvxInteraction UpdateToolbarInteraction => _updateToolbarInteraction;

        #endregion

        #region Commands

        IMvxCommand _editAuthorCommand;
        public IMvxCommand EditAuthorCommand => _editAuthorCommand ??= new MvxCommand(() =>
        {
            _device.HideKeyboard();
            if (IsEditing)
                Close();
            else
                IsEditing = !IsEditing;
        });

        IMvxAsyncCommand _deleteAuthorCommand;
        public IMvxAsyncCommand DeleteAuthorCommand => _deleteAuthorCommand ??= new MvxAsyncCommand(DeleteAuthor);

        IMvxAsyncCommand _updateAuthorCommand;
        public IMvxAsyncCommand UpdateAuthorCommand => _updateAuthorCommand ??= new MvxAsyncCommand(UpdateAuthor);

        #endregion

        #region Constructor

        public AuthorDetailViewModel(IMvxNavigationService navigation,
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

        public override void Prepare(AuthorDetailParameter parameter)
        {
            Author = parameter.Author;
            SetInitialData();
        }

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AuthorDetailViewModel)}");

            return base.Initialize();
        }

        #endregion

        #region Public

        public bool Close()
        {
            if (IsEditing)
            {
                if (HasAuthorChanged())
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

        async Task UpdateAuthor()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;


            if (HasAuthorChanged())
            {
                Author.Name = TxtName;
                var request = new ApiAuthorRequest(TxtName);

                var author = await _thePageService.UpdateAuthor(Author.Id, request);
                if (author != null)
                {
                    _userInteraction.ToastMessage("Author updated", EToastType.Success);

                    _updateToolbarInteraction.Raise();
                }

                else
                    _userInteraction.Alert("Failure updating author");
            }

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

                var result = await _thePageService.DeleteAuthor(Author);

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

        bool HasAuthorChanged()
        {
            TxtName = TxtName.Trim();
            return !Author.Name.Equals(TxtName);
        }

        void SetInitialData()
        {
            TxtName = Author.Name;
            IsEditing = false;
        }

        #endregion
    }
}