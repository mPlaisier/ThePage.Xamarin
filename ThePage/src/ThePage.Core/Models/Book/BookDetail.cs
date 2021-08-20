using System.Collections.Generic;

namespace ThePage.Core
{
    public class BookDetail
    {
        #region Properties

        public string Id { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }

        public List<Genre> Genres { get; set; }

        public string ISBN { get; set; }

        public bool Owned { get; set; }

        public bool Read { get; set; }

        public long? Pages { get; set; }

        public bool Ebook { get; set; }

        public ImageLinks Images { get; set; }

        #endregion
    }
}
