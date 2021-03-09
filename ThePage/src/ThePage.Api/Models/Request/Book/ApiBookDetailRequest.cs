using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApiBookDetailRequest
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("author")]
        public string AuthorId { get; internal set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; internal set; }

        [JsonProperty("isbn")]
        public string ISBN { get; internal set; }

        [JsonProperty("owned")]
        public bool? Owned { get; internal set; }

        [JsonProperty("read")]
        public bool? Read { get; internal set; }

        [JsonProperty("pages")]
        public long? Pages { get; internal set; }

        [JsonProperty("ebook")]
        public bool? Ebook { get; internal set; }

        [JsonProperty("olkey")]
        public string Olkey { get; internal set; }

        [JsonProperty("olcover")]
        public Olcover OlCover { get; internal set; }

        #endregion

        public class Builder
        {
            bool isEmpty = true;
            ApiBookDetailRequest _apiRequest;

            #region Public

            public Builder SetId(string id)
            {
                _apiRequest.Id = id;
                return this;
            }

            public Builder SetTitle(string title)
            {
                isEmpty = false;

                _apiRequest.Title = title;
                return this;
            }

            public Builder SetAuthor(string authorId)
            {
                isEmpty = false;

                _apiRequest.AuthorId = authorId;
                return this;
            }

            public Builder SetGenres(List<string> genres)
            {
                isEmpty = false;

                _apiRequest.Genres = genres;
                return this;
            }

            public Builder SetPages(long pages)
            {
                isEmpty = false;

                _apiRequest.Pages = pages;
                return this;
            }

            public Builder SetOwned(bool owned)
            {
                isEmpty = false;

                _apiRequest.Owned = owned;
                return this;
            }

            public Builder SetRead(bool read)
            {
                isEmpty = false;

                _apiRequest.Read = read;
                return this;
            }

            public Builder SetEbook(bool? ebook)
            {
                isEmpty = false;

                _apiRequest.Ebook = ebook;
                return this;
            }

            public Builder SetIsbn(long isbn)
            {
                isEmpty = false;

                _apiRequest.ISBN = isbn.ToString();
                return this;
            }

            public Builder SetOlKey(string olKey)
            {
                isEmpty = false;

                _apiRequest.Olkey = olKey;
                return this;
            }

            public Builder SetOlCover(Olcover olCover)
            {
                isEmpty = false;

                _apiRequest.OlCover = olCover;
                return this;
            }

            public ApiBookDetailRequest Build()
            {
                return isEmpty ? null : _apiRequest;
            }

            #endregion

            #region Constructor

            public Builder()
            {
                _apiRequest = new ApiBookDetailRequest();
            }

            #endregion
        }
    }
}