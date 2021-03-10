using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellGenreSelect : MvxNotifyPropertyChanged, ICellBaseSelect<ApiGenre>
    {
        #region Properties

        public ApiGenre Item { get; set; }

        public bool IsSelected { get; set; }

        public string IconSelected => "ic_check";

        #endregion

        #region Contructor

        public CellGenreSelect(ApiGenre genre, bool isSelected = false)
        {
            Item = genre;
            IsSelected = genre != null && isSelected;
        }

        #endregion
    }
}