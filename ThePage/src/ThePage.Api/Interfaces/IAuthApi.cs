using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IAuthApi
    {
        [Post("/login")]
        Task<ApiResponseUser> Login([Body] ApiRequestUser request);
    }
}
