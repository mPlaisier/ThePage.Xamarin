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
    public class AddAuthorViewModel : BaseViewModelResult<bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string Title => "New Author";

        public string LblName => "Name:";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtName { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblBtn => "Add author";

        #endregion

        #region Commands

        IMvxCommand _addAuthorCommand;
        public IMvxCommand AddAuthorCommand => _addAuthorCommand ??= new MvxCommand(async () =>
        {
            _device.HideKeyboard();
            await AddAuthor();
        });

        #endregion

        #region Constructor

        public AddAuthorViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddAuthorViewModel)}");

            return base.Initialize();
        }

        #endregion


        #region Private

        async Task AddAuthor()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _thePageService.AddAuthor(new Author(TxtName.Trim()));

            if (result)
            {
                _userInteraction.ToastMessage("Author added");
                await _navigation.Close(this, true);
            }
            else
            {
                _userInteraction.Alert("Failure adding author");
                IsLoading = false;
            }
        }

        #endregion
    }
}