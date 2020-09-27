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

        #region public

        public override bool Equals(object obj)
        {
            return obj is ApiBook item && Id.Equals(item.Id) && Title.Equals(item.Title);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}