using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiGenreResponse : ApiBasePageResponse<ApiGenre>
    {
    }

    public class ApiGenre
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion
    }
}