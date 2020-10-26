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
    public class AddGenreViewModel : BaseViewModelResult<string>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "New Genre";

        public string LblName => "Name:";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtName { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblBtn => "Add genre";

        #endregion

        #region Commands

        IMvxCommand _addGenreCommand;
        public IMvxCommand AddGenreCommand => _addGenreCommand ??= new MvxCommand(async () =>
        {
            _device.HideKeyboard();
            await AddGenre();
        });

        #endregion

        #region Constructor

        public AddGenreViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
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
            Analytics.TrackEvent($"Initialize {nameof(AddGenreViewModel)}");

            return base.Initialize();
        }

        #endregion


        #region Private

        async Task AddGenre()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _thePageService.AddGenre(new ApiGenreRequest(TxtName.Trim()));
            if (result != null)
            {
                _userInteraction.ToastMessage("Genre added");
                await _navigation.Close(this, result.Id);
            }
            else
            {
                _userInteraction.Alert("Failure adding genre");
                IsLoading = false;
            }
        }

        #endregion
    }
}