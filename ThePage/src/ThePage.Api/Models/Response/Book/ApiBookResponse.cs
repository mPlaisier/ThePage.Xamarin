using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiBookResponse : ApiBasePageResponse<ApiBook>
    {
    }

    public class ApiBook
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public ApiAuthor Author { get; set; }

        #endregion
    }
}