using System;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBookShelfBookItem : BaseCellKeyValueListItem
    {
        #region Properties

        public EBookShelfInputType InputType => EBookShelfInputType.Book;

        public ApiBook Book { get; }

        #endregion

        #region Constructor

        public CellBookShelfBookItem(ApiBook book,
                                     Action<ICell> actionEditClick, Action<ICell> actionClick = null,
                                     string icon = "ic_delete", bool isEdit = false)
            : base(book.Title, book.Author.Name, actionEditClick, actionClick, icon, isEdit)
        {
            Book = book;
        }

        #endregion
    }
}