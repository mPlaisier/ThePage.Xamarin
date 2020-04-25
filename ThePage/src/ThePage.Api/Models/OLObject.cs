using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public partial class OLObject
    {
        [JsonProperty("publishers")]
        public List<OLPublisher> Publishers { get; set; }

        [JsonProperty("links")]
        public List<OLLink> Links { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("identifiers")]
        public OLIdentifiers Identifiers { get; set; }

        [JsonProperty("number_of_pages")]
        public int Pages { get; set; }

        [JsonProperty("cover")]
        public OLCover Cover { get; set; }

        [JsonProperty("subject_places")]
        public List<OLItem> SubjectPlaces { get; set; }

        [JsonProperty("subjects")]
        public List<OLItem> Subjects { get; set; }

        [JsonProperty("subject_people")]
        public List<OLItem> SubjectPeople { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("authors")]
        public List<OLItem> Authors { get; set; }

        [JsonProperty("publish_date")]
        public string PublishDate { get; set; }

        [JsonProperty("excerpts")]
        public List<OLExcerpt> Excerpts { get; set; }

        [JsonProperty("subject_times")]
        public List<OLItem> SubjectTimes { get; set; }
    }

    public class OLItem
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class OLCover
    {
        [JsonProperty("small")]
        public Uri Small { get; set; }

        [JsonProperty("large")]
        public Uri Large { get; set; }

        [JsonProperty("medium")]
        public Uri Medium { get; set; }
    }

    public class OLExcerpt
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class OLIdentifiers
    {
        [JsonProperty("isbn_13")]
        public List<string> Isbn13 { get; set; }

        [JsonProperty("openlibrary")]
        public List<string> Openlibrary { get; set; }

        [JsonProperty("isbn_10")]
        public List<string> Isbn10 { get; set; }
    }

    public class OLLink
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class OLPublisher
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
