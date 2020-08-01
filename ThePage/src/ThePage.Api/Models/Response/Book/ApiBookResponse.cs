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
        public string Id { get; }

        [JsonProperty("title")]
        public string Title { get; }

        [JsonProperty("author")]
        public ApiBookAuthor Author { get; }

        #endregion
    }

    public class ApiBookAuthor
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        #endregion
    }
}