using System.Threading.Tasks;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public static class AuthManager
    {
        #region Properties

        static readonly IAuthApi _authApi = RestService.For<IAuthApi>(Secrets.ThePageAPI_URL);

        #endregion

        #region Login

        public static async Task<ApiResponseUser> Login(ApiRequestUser request)
        {
            var result = await _authApi.Login(request);

            return result;
        }

        public static async Task<ApiTokens> RefreshTokens(string refreshtoken)
        {
            var result = await _authApi.RefreshToken(new ApiRequestToken(refreshtoken));

            return result;
        }

        #endregion
    }
}