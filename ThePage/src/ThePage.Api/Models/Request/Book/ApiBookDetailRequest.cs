using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiBookDetailRequest
    {
        #region Properties

        [JsonProperty("title")]
        public string Title { get; }

        [JsonProperty("author")]
        public ApiAuthor Author { get; }

        [JsonProperty("genres")]
        public List<ApiGenre> Genres { get; }

        [JsonProperty("isbn")]
        public string ISBN { get; }

        [JsonProperty("owned")]
        public bool Owned { get; }

        [JsonProperty("read")]
        public bool Read { get; }

        [JsonProperty("pages")]
        public int Pages { get; }

        [JsonProperty("ebook")]
        public bool Ebook { get; }

        [JsonProperty("olkey")]
        public string Olkey { get; }

        [JsonProperty("olcover")]
        public List<Dictionary<string, string>> OlCover { get; }

        #endregion

        #region Constructor

        public ApiBookDetailRequest()
        {

        }

        public ApiBookDetailRequest(string title, ApiAuthor author, List<ApiGenre> genres, string iSBN, bool owned, bool read, int pages)
        {
            Title = title;
            Author = author;
            Genres = genres;
            ISBN = iSBN;
            Owned = owned;
            Read = read;
            Pages = pages;
        }

        public ApiBookDetailRequest(string title, ApiAuthor author, List<ApiGenre> genres, string iSBN, bool owned, bool read, int pages, bool ebook, string olkey, List<Dictionary<string, string>> olCover)
        {
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
}