using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiRegisterRequest
    {
        #region Properties

        [JsonProperty("username")]
        public string Username { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("email")]
        public string Email { get; }

        [JsonProperty("password")]
        public string Password { get; }

        #endregion

        #region Constructor

        public ApiRegisterRequest(string username, string name, string email, string password)
        {
            Username = username;
            Name = name;
            Email = email;
            Password = password;
        }

        #endregion
    }
}