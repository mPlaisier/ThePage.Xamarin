using System;
using ThePage.Api;

namespace ThePage.Core
{
    public class BookCell : BaseCell
    {
        #region Properties

        public string Id { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }

        #endregion

        #region Constructor

        public BookCell()
        {

        }

        public BookCell(string id, string title, Author author)
        {
            Id = id;
            Title = title;
            Author = author;
        }

        #endregion
    }
}