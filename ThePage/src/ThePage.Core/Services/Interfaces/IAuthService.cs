using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IAuthService
    {
        #region Login

        Task<bool> Login(string username, string password);

        #endregion
    }
}