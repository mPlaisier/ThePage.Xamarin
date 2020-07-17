using System.Threading.Tasks;

namespace ThePage.Core
{
    public interface IAuthService
    {
        #region Login

        Task<bool> Login(string username, string password);

        Task<string> GetSessionToken();

        #endregion
    }
}