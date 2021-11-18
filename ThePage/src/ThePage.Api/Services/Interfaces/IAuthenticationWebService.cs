using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface IAuthenticationWebService
    {
        Task<string> GetAccessToken();
        Task<bool> Login(string username, string password);
        Task Logout();
        Task<bool> Register(string username, string name, string email, string password);
    }
}