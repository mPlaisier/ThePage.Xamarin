using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiTokenRequest
    {
        #region Properties

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; }

        #endregion

        #region Constructor

        public ApiTokenRequest(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        #endregion
    }
}