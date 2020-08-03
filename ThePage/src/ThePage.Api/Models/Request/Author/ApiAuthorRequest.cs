using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiAuthorRequest
    {
        #region Properties

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; }

        [JsonProperty("olkey", NullValueHandling = NullValueHandling.Ignore)]
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