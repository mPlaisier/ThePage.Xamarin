using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class GoogleBooksResult
    {
        [JsonProperty("kind")]
        public string Kind { get; internal set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; internal set; }

        [JsonProperty("items")]
        public List<GoogleBook> Books { get; internal set; }
    }

    public class IndustryIdentifier
    {
        [JsonProperty("type")]
        public string Type { get; internal set; }

        [JsonProperty("identifier")]
        public string Identifier { get; internal set; }
    }

    public class ReadingModes
    {
        [JsonProperty("text")]
        public bool Text { get; internal set; }

        [JsonProperty("image")]
        public bool Image { get; internal set; }
    }

    public class ImageLinks
    {
        #region Properties

        [JsonProperty("smallThumbnail")]
        public string SmallThumbnail { get; internal set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; internal set; }

        [JsonProperty("small")]
        public string Small { get; internal set; }

        [JsonProperty("medium")]
        public string Medium { get; internal set; }

        [JsonProperty("large")]
        public string Large { get; internal set; }

        [JsonProperty("extraLarge")]
        public string ExtraLarge { get; internal set; }

        #endregion

        #region Public

        public string GetImageUrl()
            => Thumbnail ?? Small ?? SmallThumbnail ?? Medium ?? Large ?? ExtraLarge ?? null;

        #endregion
    }

    public class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("industryIdentifiers")]
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; }

        [JsonProperty("mainCategory")]
        public string MainCategory { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty("ratingsCount")]
        public int RatingsCount { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("infoLink")]
        public string InfoLink { get; set; }

        [JsonProperty("canonicalVolumeLink")]
        public string CanonicalVolumeLink { get; set; }
    }

    public class SaleInfo
    {
        [JsonProperty("country")]
        public string Country { get; internal set; }

        [JsonProperty("saleability")]
        public string Saleability { get; internal set; }

        [JsonProperty("isEbook")]
        public bool IsEbook { get; internal set; }
    }

    public class Epub
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; internal set; }
    }

    public class Pdf
    {
        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; internal set; }
    }

    public class Dimensions
    {
        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("thickness")]
        public string Thickness { get; set; }
    }

    public class AccessInfo
    {
        [JsonProperty("country")]
        public string Country { get; internal set; }

        [JsonProperty("viewability")]
        public string Viewability { get; internal set; }

        [JsonProperty("embeddable")]
        public bool Embeddable { get; internal set; }

        [JsonProperty("publicDomain")]
        public bool PublicDomain { get; internal set; }

        [JsonProperty("textToSpeechPermission")]
        public string TextToSpeechPermission { get; internal set; }

        [JsonProperty("epub")]
        public Epub Epub { get; internal set; }

        [JsonProperty("pdf")]
        public Pdf Pdf { get; internal set; }

        [JsonProperty("webReaderLink")]
        public string WebReaderLink { get; internal set; }

        [JsonProperty("accessViewStatus")]
        public string AccessViewStatus { get; internal set; }

        [JsonProperty("quoteSharingAllowed")]
        public bool QuoteSharingAllowed { get; internal set; }
    }

    public class SearchInfo
    {
        [JsonProperty("textSnippet")]
        public string TextSnippet { get; internal set; }
    }

    public class GoogleBook
    {
        #region Properties

        [JsonProperty("kind")]
        public string Kind { get; internal set; }

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("etag")]
        public string Etag { get; internal set; }

        [JsonProperty("selfLink")]
        public string SelfLink { get; internal set; }

        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; internal set; }

        [JsonProperty("saleInfo")]
        public SaleInfo SaleInfo { get; internal set; }

        [JsonProperty("accessInfo")]
        public AccessInfo AccessInfo { get; internal set; }

        [JsonProperty("searchInfo")]
        public SearchInfo SearchInfo { get; internal set; }

        #endregion

        #region Public

        public string GetImageUrl()
        {
            return VolumeInfo?.ImageLinks?.GetImageUrl();
        }

        #endregion
    }
}