using MvvmCross.Navigation;

namespace ThePage.Core.ViewModels.Main
{
    public class MainContainerViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly IUserInteraction _userInteraction;
        readonly IAuthService _authService;

        #region Properties

        public override string LblTitle => "";

        public bool IsLogOut { get; set; } = false;

        #endregion

        #region Constructor

        public MainContainerViewModel(IMvxNavigationService navigationService, IUserInteraction userInteraction, IAuthService authService)
        {
            _navigationService = navigationService;
            _userInteraction = userInteraction;
            _authService = authService;
        }

        #endregion

        public void LogOutUser()
        {
            _userInteraction.Confirm("Do you want to logout?", () =>
            {
                _authService.Logout().Forget();

                _navigationService.Navigate<LoginViewModel>();
            });
        }
    }
}