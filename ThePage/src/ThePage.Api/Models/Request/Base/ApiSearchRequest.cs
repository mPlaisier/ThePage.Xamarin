using Newtonsoft.Json;

namespace ThePage.Api
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiSearchRequest
    {
        #region Properties

        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("page")]
        public int? Page { get; set; }

        #endregion

        #region Constructor

        public ApiSearchRequest(int? page, string search)
        {
            Page = page;
            Search = search;
        }

        #endregion
    }
}
