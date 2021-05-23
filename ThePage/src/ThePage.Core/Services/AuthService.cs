using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using ThePage.Api;

namespace ThePage.Core
{
    public class AuthService : IAuthService
    {
        readonly IExceptionService _exceptionService;

        const string SESSION_KEY = "LoginKey";

        #region Constructor

        public AuthService(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;

            Barrel.ApplicationId = "thepageapplication";
            Barrel.EncryptionKey = "encryptionKey";
        }

        #endregion

        #region Login

        public async Task<bool> Login(string username, string password)
        {
            ApiUserReponse result = null;
            try
            {
                result = await AuthManager.Login(new ApiUserRequest(username, password));
                handleSuccessfullLogin(result);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleAuthException(ex, "Login");
            }

            return result != null;
        }

        public async Task<bool> IsAuthenticated()
        {
            return await GetSessionToken() != null;
        }

        public async Task Logout()
        {
            try
            {
                var refreshtoken = await GetSessionToken();
                HandleCloseSession();

                await AuthManager.Logout(refreshtoken);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleAuthException(ex, "Logout");
            }
        }

        public async Task<bool> Register(string username, string name, string email, string password)
        {
            ApiUserReponse result = null;
            try
            {
                result = await AuthManager.Register(new ApiRegisterRequest(username, name, email, password));
                handleSuccessfullLogin(result);
            }
            catch (Exception ex)
            {
                _exceptionService.HandleAuthException(ex, "UpdateSessionToken");
            }

            return result != null;
        }

        public async Task<string> GetSessionToken()
        {
            string token = null;
            if (Barrel.Current.Exists(SESSION_KEY) && !Barrel.Current.IsExpired(SESSION_KEY))
            {
                var result = Barrel.Current.Get<ApiTokens>(SESSION_KEY);
                token = result.Access.Expires > DateTime.UtcNow
                    ? result.Access.Token
                    : await UpdateSessionToken(result.Refresh);
            }

            if (token != null)
                return token;

            //Procedure when user session is expired
            HandleCloseSession();

            return null;
        }

        #endregion

        #region Private

        void handleSuccessfullLogin(ApiUserReponse response)
        {
            Barrel.Current.Add(SESSION_KEY, response.Tokens, TimeSpan.FromDays(30));
        }

        async Task<string> UpdateSessionToken(TokenObject token)
        {
            if (token.Expires > DateTime.UtcNow)
            {
                try
                {
                    var result = await AuthManager.RefreshTokens(token.Token);
                    if (result != null)
                    {
                        Barrel.Current.Add(SESSION_KEY, result, TimeSpan.FromDays(30));
                        return result.Access.Token;
                    }
                }
                catch (Exception ex)
                {
                    _exceptionService.HandleAuthException(ex, "UpdateSessionToken");
                }
            }
            return null;
        }

        void HandleCloseSession()
        {
            Barrel.Current.EmptyAll();
        }

        #endregion
    }
}