using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiUserRequest
    {
        #region Properties

        [JsonProperty("username")]
        public string Username { get; }

        [JsonProperty("password")]
        public string Password { get; }

        #endregion

        #region Constructor

        public ApiUserRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        #endregion
    }
}