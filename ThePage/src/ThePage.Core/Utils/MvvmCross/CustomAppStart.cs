using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels.Main;

namespace ThePage.Core
{
    public class CustomAppStart : MvxAppStart
    {
        readonly IMvxNavigationService _navigationService;
        readonly IAuthService _authService;
        readonly IExceptionService _exceptionService;

        public CustomAppStart(IMvxApplication app,
                              IMvxNavigationService navigationService,
                              IAuthService authService,
                              IExceptionService exceptionService)
            : base(app, navigationService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _exceptionService = exceptionService;
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            var isAuthenticated = false;
            try
            {
                isAuthenticated = await _authService.IsAuthenticated();
            }
            catch (Exception exception)
            {
                _exceptionService.AddExceptionForLogging(exception, new Dictionary<string, string>
                {
                    { "Service", nameof(AuthService) },
                    { "AppStart", nameof(CustomAppStart) }
                });
            }

            if (isAuthenticated)
                await _navigationService.Navigate<MainViewModel>();
            else
                await _navigationService.Navigate<LoginViewModel>();
        }
    }
}