using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class ApiBasePageResponse<T>
    {
        #region Properties

        [JsonProperty("docs")]
        public List<T> Docs { get; internal set; }

        [JsonProperty("totalDocs")]
        public int TotalDocs { get; internal set; }

        [JsonProperty("limit")]
        public int Limit { get; internal set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; internal set; }

        [JsonProperty("page")]
        public int Page { get; internal set; }

        [JsonProperty("pagingCounter")]
        public int PagingCounter { get; internal set; }

        [JsonProperty("hasPrevPage")]
        public bool HasPrevPage { get; internal set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; internal set; }

        [JsonProperty("prevPage")]
        public int? PrevPage { get; internal set; }

        [JsonProperty("nextPage")]
        public int? NextPage { get; internal set; }

        #endregion
    }
}