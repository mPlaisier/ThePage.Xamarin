using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IAuthApi
    {
        [Post("/auth/login")]
        Task<ApiUserReponse> Login([Body] ApiUserRequest request);

        [Post("/auth/logout")]
        Task Logout([Body] ApiTokenRequest request);

        [Post("/auth/register")]
        Task<ApiUserReponse> Register([Body] ApiRegisterRequest request);

        [Post("/auth/refresh-tokens")]
        Task<ApiTokens> RefreshToken([Body] ApiTokenRequest request);
    }
}