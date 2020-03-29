using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddGenreViewModel : BaseViewModelResult<bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string Title => "New Genre";

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

            var result = await _thePageService.AddGenre(new Genre(TxtName.Trim()));

            if (result)
            {
                _userInteraction.ToastMessage("Genre added");
                await _navigation.Close(this, true);
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