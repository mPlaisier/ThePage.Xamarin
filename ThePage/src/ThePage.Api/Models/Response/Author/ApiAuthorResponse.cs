using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiAuthorResponse : ApiBasePageResponse<ApiAuthor>
    {
    }

    public class ApiAuthor
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("olkey")]
        public string Olkey { get; set; }

        #endregion
    }
}