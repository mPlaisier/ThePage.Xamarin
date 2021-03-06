using System;
using System.Threading.Tasks;
using CBP.Extensions;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;
using ThePage.Core.ViewModels.Main;

namespace ThePage.Core
{
    public class RegisterViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly IAuthService _authService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Sign up";

        public string LblDisclaimer => "Create an account";

        public string LblUsernameHint => "Username";

        public string Username { get; set; }

        public string LblEmailHint => "Email";

        public string Email { get; set; }

        public string LblNameHint => "First and last name";

        public string Name { get; set; }

        public string LblPasswordHint => "Password";

        public string Password { get; set; }

        public string LblRepeatPasswordHint => "Confirm password";

        public string RepeatPassword { get; set; }

        public string LblBtnRegister => "Register";

        public bool BtnRegisterEnabled => ValidateInput();

        public string LblLogin => "Already have an account? Log in";

        #endregion

        #region Commands

        MvxCommand _registerCommand;
        public IMvxCommand RegisterCommand => _registerCommand = _registerCommand ?? new MvxCommand(() =>
        {
            _device.HideKeyboard();
            OnRegisterClick().Forget();
        });

        MvxCommand _loginCommand;
        public IMvxCommand LoginCommand => _loginCommand = _loginCommand ?? new MvxCommand(() =>
        {
            _navigationService.Navigate<LoginViewModel>();
        });

        #endregion

        #region Constructor

        public RegisterViewModel(IMvxNavigationService navigationService,
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
            Analytics.TrackEvent($"Initialize {nameof(RegisterViewModel)}");

            return base.Initialize();
        }

        #endregion

        #region Private

        async Task OnRegisterClick()
        {
            IsLoading = true;

            var success = await _authService.Register(Username, Name, Email, Password);
            if (success)
            {
                _userInteraction.ToastMessage("Registration successfull, logging in...", EToastType.Success);
                await _navigationService.Navigate<MainViewModel>();
            }
            else
                IsLoading = false;
        }

        bool ValidateInput()
        {
            if (Username.IsNullOrEmpty()
               || Email.IsNullOrEmpty()
               || Name.IsNullOrEmpty()
               || Password.IsNullOrEmpty()
               || RepeatPassword.IsNullOrEmpty())
                return false;

            if (!Password.Equals(RepeatPassword))
                return false;

            //TODO password min req

            return true;
        }

        #endregion
    }
}