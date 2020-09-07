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
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => string.Empty;

        public string LblUsernameHint => "Username";

        public string Username { get; set; }

        public string LblPasswordHint => "Password";

        public string Password { get; set; }

        public string LblBtnLogin => "Sign in";

        public string LblRegister => "Don't have an account? Sign up";

        public bool BtnLoginEnabled => !Username.IsNullOrEmpty() && !Password.IsNullOrEmpty();

        #endregion

        #region Commands

        MvxCommand _loginCommand;
        public IMvxCommand LoginCommand => _loginCommand = _loginCommand ?? new MvxCommand(() =>
        {
            _device.HideKeyboard();
            OnloginClick().Forget();
        });

        MvxCommand _registerCommand;
        public IMvxCommand RegisterCommand => _registerCommand = _registerCommand ?? new MvxCommand(() =>
        {
            _navigationService.Navigate<RegisterViewModel>();
        });

        #endregion

        #region Constructor

        public LoginViewModel(IMvxNavigationService navigationService,
                              IAuthService authService,
                              IUserInteraction userInteraction,
                              IDevice device)
        {
            _navigationService = navigationService;
            _authService = authService;
            _userInteraction = userInteraction;
            _device = device;
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
            IsLoading = true;

            var success = await _authService.Login(Username, Password);
            if (success)
            {
                _userInteraction.ToastMessage("Success", EToastType.Success);
                await _navigationService.Navigate<MainViewModel>();
                await _navigationService.Close(this);
            }
            else
                IsLoading = false;
        }

        #endregion
    }
}