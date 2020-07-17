using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.LiteDB;
using MvvmCross;
using Refit;
using ThePage.Api;

namespace ThePage.Core
{
    public class AuthService : IAuthService
    {
        readonly IUserInteraction _userInteraction;
        const string LoginKey = "LoginKey";

        #region Properties

        public bool IsLoggedIn { get; internal set; }

        #endregion

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
            ApiResponseUser result = null;
            try
            {
                result = await AuthManager.Login(new ApiRequestUser(username, password));
                handleSuccessfullLogin(result);
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
                var result = Barrel.Current.Get<ApiResponseUser>(LoginKey);
                token = result.Tokens.Access.Expires < DateTime.Now
                    ? result.Tokens.Access.Token
                    : await UpdateSessionToken(result.Tokens.Refresh);
            }

            if (token != null)
                return token;

            //Procedure when user session is expired
            HandleSessionExpired();

            return null;
        }

        #endregion

        #region Private

        void handleSuccessfullLogin(ApiResponseUser response)
        {
            IsLoggedIn = true;
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

        void HandleSessionExpired()
        {
            IsLoggedIn = false;

            Barrel.Current.EmptyAll();
        }

        //TODO Move to General handle exception class
        void HandleException(Exception ex)
        {
            Crashes.TrackError(ex);

            if (ex is ApiException apiException)
            {
                if (apiException.StatusCode == HttpStatusCode.NotFound)
                {
                    _userInteraction.Alert("Item not found", null, "Error");
                }
                else if (apiException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _userInteraction.Alert("Incorrect username or password", null, "Error");
                }
            }
            else
            {
                _userInteraction.Alert(ex.Message, null, "Error");
            }
        }

        #endregion
    }
}