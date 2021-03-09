using System;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class Genre
    {
        #region Properties

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion

        #region Constructor

        public Genre()
        {
        }

        public Genre(string name)
        {
            Name = name;
        }

        public Genre(string id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion

        #region Equals and GetHasCode

        public override bool Equals(object obj)
        {
            return obj != null && obj is Genre genre && (Id == genre.Id) && Name.Equals(genre.Name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        #endregion
    }
}
