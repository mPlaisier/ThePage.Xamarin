using System.Threading.Tasks;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public static class AuthManager
    {
        #region Login

        public static async Task<ApiUserReponse> Login(ApiUserRequest request)
        {
            var _authApi = RestService.For<IAuthApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL));
            var result = await _authApi.Login(request);

            return result;
        }

        public static async Task<ApiUserReponse> Register(ApiRegisterRequest request)
        {
            var _authApi = RestService.For<IAuthApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL));
            var result = await _authApi.Register(request);

            return result;
        }

        public static async Task<ApiTokens> RefreshTokens(string refreshtoken)
        {
            var _authApi = RestService.For<IAuthApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL));
            var result = await _authApi.RefreshToken(new ApiTokenRequest(refreshtoken));

            return result;
        }

        #endregion
    }
}