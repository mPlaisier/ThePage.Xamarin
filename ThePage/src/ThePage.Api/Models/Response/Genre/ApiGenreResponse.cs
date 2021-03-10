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
        public string Id { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            return obj is ApiGenre item && Id.Equals(item.Id) && Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}