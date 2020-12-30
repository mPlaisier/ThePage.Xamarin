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
        public string Olkey { get; }

        #endregion

        #region Constructor

        public ApiAuthorRequest(string name)
        {
            Name = name;
        }

        public ApiAuthorRequest(string name, string olkey)
        {
            Name = name;
            Olkey = olkey;
        }

        public ApiAuthorRequest(ApiAuthor apiAuthor)
        {
            Name = apiAuthor.Name;
            Olkey = apiAuthor.Olkey;
        }

        #endregion
    }
}