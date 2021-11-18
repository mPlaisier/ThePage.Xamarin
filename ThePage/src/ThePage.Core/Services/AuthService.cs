using System;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    [ThePageLazySingletonService]
    public class AuthService : IAuthService
    {
        readonly IExceptionService _exceptionService;
        readonly IAuthenticationWebService _authenticationWebService;

        #region Constructor

        public AuthService(IExceptionService exceptionService, IAuthenticationWebService authenticationWebService)
        {
            _exceptionService = exceptionService;
            _authenticationWebService = authenticationWebService;
        }

        #endregion

        #region Public

        public async Task<bool> Login(string username, string password)
        {
            var result = false;
            try
            {
                result = await _authenticationWebService.Login(username, password);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleAuthException(ex, nameof(Login));
            }

            return result;
        }

        public async Task<bool> IsAuthenticated()
        {
            return await _authenticationWebService.GetAccessToken() != null;
        }

        public async Task Logout()
        {
            try
            {
                await _authenticationWebService.Logout();
            }
            catch (Exception ex)
            {
                _exceptionService.HandleAuthException(ex, nameof(Logout));
            }
        }

        public async Task<bool> Register(string username, string name, string email, string password)
        {
            var result = false;
            try
            {
                result = await _authenticationWebService.Register(username, name, email, password);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleAuthException(ex, nameof(Register));
            }

            return result;
        }

        #endregion


    }
}