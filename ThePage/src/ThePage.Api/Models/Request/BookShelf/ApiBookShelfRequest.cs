using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiBookShelfRequest
    {
        #region Properties

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("books", NullValueHandling = NullValueHandling.Ignore)]
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