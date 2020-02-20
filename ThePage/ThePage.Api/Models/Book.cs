using System;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class Book
    {
        #region Properties

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        #endregion

        #region Constructor

        public Book()
        {
        }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }

        #endregion
    }
}
