using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IAuthApi
    {
        [Post("/auth/login")]
        Task<ApiResponseUser> Login([Body] ApiRequestUser request);

        [Post("/refresh-tokens")]
        Task<ApiTokens> RefreshToken([Body] ApiRequestToken request);
    }
}
