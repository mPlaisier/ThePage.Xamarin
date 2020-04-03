using System;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class Author
    {
        #region Properties

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion

        #region Constructor

        public Author()
        {
        }

        public Author(string name)
        {
            Name = name;
        }

        public Author(string id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion
    }
}
