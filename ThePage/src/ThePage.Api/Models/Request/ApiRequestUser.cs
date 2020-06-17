using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiRequestUser
    {
        #region Properties

        [JsonProperty("username")]
        public string Username { get; }

        [JsonProperty("password")]
        public string Password { get; }

        #endregion

        #region Constructor

        public ApiRequestUser(string username, string password)
        {
            Username = username;
            Password = password;
        }

        #endregion
    }
}