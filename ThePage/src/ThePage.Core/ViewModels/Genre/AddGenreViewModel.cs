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

        public bool IsValid => !string.IsNullOrEmpty(TxtName);

        public string LblBtn => "Add genre";

        #endregion

        #region Commands

        IMvxCommand _addGenreCommand;
        public IMvxCommand AddGenreCommand => _addGenreCommand ??= new MvxCommand(async () =>
        {
            await AddGenre();
        });

        #endregion

        #region Constructor

        public AddGenreViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
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

            var result = await _thePageService.AddGenre(new Genre(TxtName));

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