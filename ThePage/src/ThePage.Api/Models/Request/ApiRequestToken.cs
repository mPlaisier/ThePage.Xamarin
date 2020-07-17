using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiRequestToken
    {
        #region Properties

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        #endregion

        #region Constructor

        public ApiRequestToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        #endregion
    }
}