using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellBookSelect : MvxNotifyPropertyChanged, ICellBaseSelect<ApiBook>
    {
        #region Properties

        public ApiBook Item { get; set; }

        public bool IsSelected { get; set; }

        public string IconSelected => "ic_check";

        #endregion

        #region Contructor

        public CellBookSelect(ApiBook book, bool isSelected = false)
        {
            Item = book;

            IsSelected = book != null ? isSelected : false;
        }

        #endregion
    }
}