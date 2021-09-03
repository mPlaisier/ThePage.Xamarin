using System;

namespace ThePage.Core
{
    public class CellBookShelfBookItem : BaseCellKeyValueListItem
    {
        #region Properties

        public EBookShelfInputType InputType => EBookShelfInputType.Book;

        public Book Book { get; }

        #endregion

        #region Constructor

        public CellBookShelfBookItem(Book book,
                                     Action<ICell> actionEditClick, Action<ICell> actionClick = null,
                                     string icon = Constants.ICON_DELETE, bool isEdit = false)
            : base(book.Title, book.Author?.Name, actionEditClick, actionClick, icon, isEdit)
        {
            Book = book;
        }

        #endregion
    }
}