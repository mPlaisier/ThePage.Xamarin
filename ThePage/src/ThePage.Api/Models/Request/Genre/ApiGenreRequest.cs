using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiGenreRequest
    {
        #region Properties

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        #endregion

        #region Constructor

        public ApiGenreRequest(string name)
        {
            Name = name;
        }

        #endregion
    }
}