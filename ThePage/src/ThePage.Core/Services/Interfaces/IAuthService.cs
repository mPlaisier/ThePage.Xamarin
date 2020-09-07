using System.Threading.Tasks;

namespace ThePage.Core
{
    public interface IAuthService
    {
        #region Login

        Task<bool> Login(string username, string password);

        Task<bool> IsAuthenticated();

        Task Logout();

        Task<bool> Register(string username, string name, string email, string password);

        Task<string> GetSessionToken();

        #endregion
    }
}