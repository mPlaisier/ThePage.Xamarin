using System.Collections.Generic;
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

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        #endregion

        #region Constructor

        public Book()
        {
        }

        public Book(string title, string author, List<string> genres)
        {
            Title = title;
            Author = author;
            Genres = genres;
        }

        public Book(string id, string title, string author, List<string> genres) : this(title, author, genres)
        {
            Id = id;
        }

        #endregion
    }
}
