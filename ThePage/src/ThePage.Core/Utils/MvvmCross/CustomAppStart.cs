using System;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels.Main;

namespace ThePage.Core
{
    public class CustomAppStart : MvxAppStart
    {
        readonly IMvxNavigationService _mvxNavigationService;
        readonly IAuthService _authService;

        public CustomAppStart(IMvxApplication app,
                        IMvxNavigationService mvxNavigationService,
                        IAuthService authService)
            : base(app, mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
            _authService = authService;
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                var isAuthenticated = await _authService.IsAuthenticated();

                if (isAuthenticated)
                {
                    await _mvxNavigationService.Navigate<MainViewModel>();
                }
                else
                {
                    await _mvxNavigationService.Navigate<LoginViewModel>();
                }
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem navigating to ViewModel {0}", typeof(MvxViewModel).Name);
            }
        }
    }
}