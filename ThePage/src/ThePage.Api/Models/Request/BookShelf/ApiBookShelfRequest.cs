using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiBookShelfRequest
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("books")]
        public List<string> Books { get; set; }

        #endregion

        #region Constructor

        public ApiBookShelfRequest(string id, string name, List<string> books)
        {
            Id = id;
            Name = name;
            Books = books;
        }

        #endregion
    }
}