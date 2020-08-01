using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiAuthorRequest
    {
        #region Properties

        [JsonProperty("name")]
        public string Name { get; }

        #endregion

        #region Constructor

        public ApiAuthorRequest(string name)
        {
            Name = name;
        }

        #endregion
    }
}