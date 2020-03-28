using Newtonsoft.Json;

namespace ThePage.Api
{
    public class Genre
    {
        #region Properties

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion

        #region Constructor

        public Genre()
        {
        }

        public Genre(string name)
        {
            Name = name;
        }

        public Genre(string id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion
    }
}
