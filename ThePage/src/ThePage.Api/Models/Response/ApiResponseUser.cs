using System;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiResponseUser
    {
        #region Properties

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("tokens")]
        public Tokens Tokens { get; set; }

        #endregion
    }

    public class Tokens
    {
        #region Properties

        [JsonProperty("access")]
        public TokenObject Access { get; set; }

        [JsonProperty("refresh")]
        public TokenObject Refresh { get; set; }

        #endregion
    }

    public class TokenObject
    {
        #region Properties

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires")]
        public DateTimeOffset Expires { get; set; }

        #endregion
    }

    public class User
    {
        #region Properties

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        #endregion
    }
}