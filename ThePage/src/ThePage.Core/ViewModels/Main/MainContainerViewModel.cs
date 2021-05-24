using CBP.Extensions;
using MvvmCross.Navigation;

namespace ThePage.Core.ViewModels.Main
{
    public class MainContainerViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly IUserInteraction _userInteraction;
        readonly IAuthService _authService;
        readonly IExceptionService _exceptionService;

        #region Properties

        public override string LblTitle => "";

        public bool IsLogOut { get; set; } = false;

        #endregion

        #region Constructor

        public MainContainerViewModel(IMvxNavigationService navigationService,
                                      IUserInteraction userInteraction,
                                      IAuthService authService,
                                      IExceptionService exceptionService)
        {
            _navigationService = navigationService;
            _userInteraction = userInteraction;
            _authService = authService;
            _exceptionService = exceptionService;
        }

        #endregion

        #region Public

        public void LogOutUser()
        {
            _userInteraction.Confirm("Do you want to logout?", () =>
            {
                _authService.Logout().Forget();

                _navigationService.Navigate<LoginViewModel>();
            });
        }

        public void EnableExceptionLogging()
            => _exceptionService.LogginIsEnabled();

        #endregion


    }
}