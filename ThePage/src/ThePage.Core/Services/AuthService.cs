using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.LiteDB;
using MvvmCross;
using Newtonsoft.Json;
using Refit;
using ThePage.Api;

namespace ThePage.Core
{
    public class AuthService : IAuthService
    {
        readonly IUserInteraction _userInteraction;
        const string LoginKey = "LoginKey";

        #region Constructor

        public AuthService() : this(Mvx.IoCProvider.Resolve<IUserInteraction>())
        {
        }

        public AuthService(IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
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
                HandleException(ex);
            }

            return result != null;
        }

        public async Task<bool> IsAuthenticated()
        {
            return await GetSessionToken() != null;
        }

        public async Task Logout()
        {
            var refreshtoken = await GetSessionToken();
            HandleCloseSession();

            await AuthManager.Logout(refreshtoken);
        }

        public async Task<bool> Register(string username, string name, string email, string password)
        {
            ApiUserReponse result = null;
            try
            {
                result = await AuthManager.Register(new ApiRegisterRequest(username, name, email, password));
                handleSuccessfullLogin(result);
            }
            catch (ApiException ex)
            {
                ApiError error = JsonConvert.DeserializeObject<ApiError>(ex.Content);
                _userInteraction.Alert(error.Message, null, "Error");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result != null;
        }

        public async Task<string> GetSessionToken()
        {
            string token = null;
            if (Barrel.Current.Exists(LoginKey))
            {
                var result = Barrel.Current.Get<ApiTokens>(LoginKey);
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
            Barrel.Current.Add(LoginKey, response.Tokens, TimeSpan.FromDays(30));
        }

        async Task<string> UpdateSessionToken(TokenObject token)
        {
            if (token.Expires < DateTime.Now)
            {
                var result = await AuthManager.RefreshTokens(token.Token);
                if (result != null)
                {
                    Barrel.Current.Add(LoginKey, result, TimeSpan.FromDays(30));
                    return result.Access.Token;
                }
            }
            return null;
        }

        void HandleCloseSession()
        {
            Barrel.Current.EmptyAll();
        }

        //TODO Move to General handle exception class
        void HandleException(Exception ex)
        {
            if (ex is ApiException apiException)
            {
                ApiError error = JsonConvert.DeserializeObject<ApiError>(apiException.Content);

                if (apiException.StatusCode == HttpStatusCode.NotFound)
                {
                    _userInteraction.Alert("Item not found", null, "Error");
                }
                else if (apiException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _userInteraction.ToastMessage(error.Message, EToastType.Error);
                }
                else
                {
                    _userInteraction.Alert(error.Message, null, "Error");
                }
            }
            else
            {
                Crashes.TrackError(ex);

                _userInteraction.Alert(ex.Message, null, "Error");
            }
        }

        #endregion
    }
}