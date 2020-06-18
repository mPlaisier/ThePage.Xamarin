using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;
using ThePage.Core.ViewModels.Main;

namespace ThePage.Core
{
    public class LoginViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly IAuthService _authService;

        #region Properties

        public override string Title => string.Empty;

        public string Username { get; set; }

        public string Password { get; set; }

        public string LblBtnLogin => "Sign in";

        public string LblRegister => "Don't have an account? Sign up";

        public bool BtnLoginEnabled => !Username.IsNullOrEmpty() && !Password.IsNullOrEmpty();

        #endregion

        #region Commands

        private MvxAsyncCommand _loginCommand;
        public IMvxAsyncCommand LoginCommand => _loginCommand = _loginCommand ?? new MvxAsyncCommand(OnloginClick);

        private MvxCommand _registerCommand;
        public IMvxCommand RegisterCommand => _registerCommand = _registerCommand ?? new MvxCommand(OnRegisterClick);

        #endregion

        #region Constructor

        public LoginViewModel(IMvxNavigationService navigationService, IAuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;
        }

        #endregion

        #region LifeCycle

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddAuthorViewModel)}");

#if DEBUG
            Username = "destruction505";
            Password = "abc123456";
#endif
            return base.Initialize();
        }

        #endregion

        #region Private

        async Task OnloginClick()
        {
            //Start login
            IsLoading = true;

            var success = await _authService.Login(Username, Password);

            if (success)
            {
                await _navigationService.Navigate<MainViewModel>();
            }
            else
            {
                //Show error
                IsLoading = false;
            }
        }

        void OnRegisterClick()
        {
            //Navigate to Register VM
        }

        #endregion
    }
}
