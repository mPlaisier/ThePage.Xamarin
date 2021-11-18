using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface ITokenService
    {
        #region Methods

        TokenResult GetAccessToken();

        void SetSessionToken(ApiTokens tokens);

        bool ShouldRefreshToken();

        string GetRefreshToken();

        #endregion

    }
}