using System;
using System.Threading.Tasks;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class AuthenticationWebService : IAuthenticationWebService
    {
        readonly ITokenlessWebService _webService;
        readonly ITokenService _tokenService;
        readonly ILocalDatabaseService _localDatabase;

        #region Constructor

        public AuthenticationWebService(ITokenlessWebService webService, ITokenService tokenService, ILocalDatabaseService localDatabase)
        {
            _webService = webService;
            _tokenService = tokenService;
            _localDatabase = localDatabase;
        }

        #endregion

        #region Public

        public async Task<bool> Login(string username, string password)
        {
            var api = await _webService.GetApi<IAuthApi>();
            var result = await api.Login(new ApiUserRequest(username, password));
            handleSuccessfullLogin(result);

            return result != null;
        }

        public async Task Logout()
        {
            HandleCloseSession();

            var refreshtoken = _tokenService.GetRefreshToken();
            if (refreshtoken != null)
            {
                var api = await _webService.GetApi<IAuthApi>();
                await api.Logout(new ApiTokenRequest(refreshtoken));
            }
        }

        public async Task<bool> Register(string username, string name, string email, string password)
        {
            var api = await _webService.GetApi<IAuthApi>();
            var result = await api.Register(new ApiRegisterRequest(username, name, email, password));
            handleSuccessfullLogin(result);

            return result != null;
        }

        public async Task<string> GetAccessToken()
        {
            var tokenResult = _tokenService.GetAccessToken();

            if (tokenResult == null)
                return null;

            return tokenResult.ShouldRefresh
                ? await UpdateSessionToken(tokenResult.Tokens.Refresh)
                : tokenResult.Tokens.Access.Token;
        }

        #endregion

        #region Private

        void handleSuccessfullLogin(ApiUserReponse response)
        {
            _tokenService.SetSessionToken(response.Tokens);
        }

        void HandleCloseSession()
        {
            _localDatabase.ClearAll();
        }

        async Task<string> UpdateSessionToken(TokenObject token)
        {
            if (token.Expires > DateTime.UtcNow)
            {
                var api = await _webService.GetApi<IAuthApi>();
                var result = await api.RefreshToken(new ApiTokenRequest(token.Token));
                if (result != null)
                {
                    _tokenService.SetSessionToken(result);
                    return result.Access.Token;
                }
            }
            return null;
        }

        #endregion
    }
}
