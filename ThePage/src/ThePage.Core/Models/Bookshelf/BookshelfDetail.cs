using System.Collections.Generic;

namespace ThePage.Core
{
    public class BookshelfDetail
    {
        #region Properties

        public string Id { get; set; }

        public string Name { get; set; }

        public List<Book> Books { get; set; }

        #endregion
    }
}
