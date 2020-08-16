using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    //TODO check todo ignore via top level
    public class ApiBookDetailRequest
    {
        #region Properties

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; internal set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; internal set; }

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorId { get; internal set; }

        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Genres { get; internal set; }

        [JsonProperty("isbn", NullValueHandling = NullValueHandling.Ignore)]
        public string ISBN { get; internal set; }

        [JsonProperty("owned", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Owned { get; internal set; }

        [JsonProperty("read", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Read { get; internal set; }

        [JsonProperty("pages", NullValueHandling = NullValueHandling.Ignore)]
        public int? Pages { get; internal set; }

        [JsonProperty("ebook", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ebook { get; internal set; }

        [JsonProperty("olkey", NullValueHandling = NullValueHandling.Ignore)]
        public string Olkey { get; internal set; }

        [JsonProperty("olcover", NullValueHandling = NullValueHandling.Ignore)]
        public Olcover OlCover { get; internal set; }

        #endregion

        #region Constructor

        public ApiBookDetailRequest()
        {
        }

        public ApiBookDetailRequest(string id, string title, string author, List<string> genres, string iSBN, bool owned, bool read, int pages)
        {
            Id = id;
            Title = title;
            AuthorId = author;
            Genres = genres;
            ISBN = iSBN;
            Owned = owned;
            Read = read;
            Pages = pages;
        }

        public ApiBookDetailRequest(string title, string author, List<string> genres, string iSBN, bool owned, bool read, int pages, bool ebook, string olkey, Olcover olCover)
        {
            Title = title;
            AuthorId = author;
            Genres = genres;
            ISBN = iSBN;
            Owned = owned;
            Read = read;
            Pages = pages;
            Ebook = ebook;
            Olkey = olkey;
            OlCover = olCover;
        }

        public ApiBookDetailRequest(string id, string title, string author, List<string> genres, string iSBN, bool? owned, bool? read, int? pages, bool? ebook, string olkey, Olcover olCover)
        {
            Id = id;
            Title = title;
            AuthorId = author;
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

        public class Builder
        {
            string _id;
            string _title;
            string _authorId;

            List<string> _genres;
            int? _pages;

            bool? _owned;
            bool? _read;
            bool? _ebook;

            string _iSBN;
            string _olkey;
            Olcover _olCover;

            #region Public

            public Builder SetId(string id)
            {
                _id = id;
                return this;
            }

            public Builder SetTitle(string title)
            {
                _title = title;
                return this;
            }

            public Builder SetAuthor(string authorId)
            {
                _authorId = authorId;
                return this;
            }

            public Builder SetGenres(List<string> genres)
            {
                _genres = genres;
                return this;
            }

            public Builder SetPages(int? pages)
            {
                _pages = pages;
                return this;
            }

            public Builder SetOwned(bool? owned)
            {
                _owned = owned;
                return this;
            }

            public Builder SetRead(bool? read)
            {
                _read = read;
                return this;
            }

            public Builder SetEbook(bool? ebook)
            {
                _ebook = ebook;
                return this;
            }

            public Builder SetIsbn(string isbn)
            {
                _iSBN = isbn;
                return this;
            }

            public Builder SetOlKey(string olKey)
            {
                _olkey = olKey;
                return this;
            }

            public Builder SetOlCover(Olcover olCover)
            {
                _olCover = olCover;
                return this;
            }

            public ApiBookDetailRequest Build()
            {
                return new ApiBookDetailRequest(_id,
                                                _title,
                                                _authorId,
                                                _genres,
                                                _iSBN,
                                                _owned,
                                                _read,
                                                _pages,
                                                _ebook,
                                                _olkey,
                                                _olCover);
            }

            #endregion
        }
    }
}