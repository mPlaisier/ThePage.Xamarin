using Newtonsoft.Json;

namespace ThePage.Api
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiPageRequest
    {
        #region Properties

        [JsonProperty("page")]
        public int? Page { get; set; }

        #endregion

        #region Constructor

        public ApiPageRequest(int? page)
        {
            Page = page;
        }

        #endregion

    }
}
