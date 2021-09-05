using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiBookShelfResponse : ApiBasePageResponse<ApiBookShelf>
    {
    }

    public class ApiBookShelf
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("books")]
        public List<string> Books { get; set; }

        #endregion
    }
}