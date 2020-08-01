using System.Threading.Tasks;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public static class AuthManager
    {
        #region Login

        public static async Task<ApiResponseUser> Login(ApiRequestUser request)
        {
            var _authApi = RestService.For<IAuthApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL));
            var result = await _authApi.Login(request);

            return result;
        }

        public static async Task<ApiTokens> RefreshTokens(string refreshtoken)
        {
            var _authApi = RestService.For<IAuthApi>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL));
            var result = await _authApi.RefreshToken(new ApiRequestToken(refreshtoken));

            return result;
        }

        #endregion
    }
}