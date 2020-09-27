using Newtonsoft.Json;

namespace ThePage.Api
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiAuthorRequest
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

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