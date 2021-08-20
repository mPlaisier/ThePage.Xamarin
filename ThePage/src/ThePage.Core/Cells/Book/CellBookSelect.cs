using MvvmCross.ViewModels;

namespace ThePage.Core.Cells
{
    public class CellBookSelect : MvxNotifyPropertyChanged, ICellBaseSelect<Book>
    {
        #region Properties

        public Book Item { get; set; }

        public bool IsSelected { get; set; }

        public string IconSelected => Constants.ICON_CHECK;

        #endregion

        #region Contructor

        public CellBookSelect(Book book, bool isSelected = false)
        {
            Item = book;
            IsSelected = book != null && isSelected;
        }

        #endregion
    }
}