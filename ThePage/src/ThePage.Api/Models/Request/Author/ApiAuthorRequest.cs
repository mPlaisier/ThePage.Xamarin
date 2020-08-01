using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiAuthorRequest
    {
        #region Properties

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("olkey")]
        public string Olkey { get; set; }

        #endregion

        #region Constructor

        public ApiAuthorRequest(string name)
        {
            Name = name;
        }

        #endregion
    }
}