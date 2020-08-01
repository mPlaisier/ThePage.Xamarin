using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiBookDetailResponse
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; }

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
    }
}