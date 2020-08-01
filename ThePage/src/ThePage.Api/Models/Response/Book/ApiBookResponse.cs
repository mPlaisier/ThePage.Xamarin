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
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public ApiBookAuthor Author { get; set; }

        #endregion
    }

    public class ApiBookAuthor
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion
    }
}