using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiBookDetailResponse
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public ApiAuthor Author { get; set; }

        [JsonProperty("genres")]
        public List<ApiGenre> Genres { get; set; }

        [JsonProperty("isbn")]
        public string ISBN { get; set; }

        [JsonProperty("owned")]
        public bool Owned { get; set; }

        [JsonProperty("read")]
        public bool Read { get; set; }

        [JsonProperty("pages")]
        public long? Pages { get; set; }

        [JsonProperty("ebook")]
        public bool Ebook { get; set; }

        [JsonProperty("olkey")]
        public string Olkey { get; set; }

        [JsonProperty("olcover")]
        public Olcover OlCover { get; set; }

        #endregion

        #region Constructor

        public ApiBookDetailResponse()
        {
        }

        public ApiBookDetailResponse(string id, string title, ApiAuthor author, List<ApiGenre> genres, string iSBN, bool owned, bool read, int pages)
        {
            Id = id;
            Title = title;
            Author = author;
            Genres = genres;
            ISBN = iSBN;
            Owned = owned;
            Read = read;
            Pages = pages;
        }

        public ApiBookDetailResponse(string id, string title, ApiAuthor author, List<ApiGenre> genres, string iSBN, bool owned, bool read, int pages, bool ebook, string olkey, Olcover olCover)
        {
            Id = id;
            Title = title;
            Author = author;
            Genres = genres;
            ISBN = iSBN;
            Owned = owned;
            Read = read;
            Pages = pages;
            Ebook = ebook;
            Olkey = olkey;
            OlCover = olCover;
        }

        #endregion
    }

    public class Olcover
    {
        [JsonProperty("small")]
        public string Small { get; internal set; }

        [JsonProperty("medium")]
        public string Medium { get; internal set; }

        [JsonProperty("large")]
        public string Large { get; internal set; }
    }
}