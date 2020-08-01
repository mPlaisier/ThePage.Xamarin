using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiGenreRequest
    {
        #region Properties

        [JsonProperty("name")]
        public string Name { get; }

        #endregion

        #region Constructor

        public ApiGenreRequest(string name)
        {
            Name = name;
        }

        #endregion
    }
}