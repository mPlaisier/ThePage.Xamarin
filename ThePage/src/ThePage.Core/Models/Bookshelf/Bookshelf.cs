using System.Collections.Generic;

namespace ThePage.Core
{
    public class Bookshelf
    {
        #region Properties

        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> Books { get; set; }

        public int Count => Books == null ? 0 : Books.Count;

        public string LblCount => Count == 1 ? $"{Count} book" : $"{Count} books";

        #endregion
    }
}