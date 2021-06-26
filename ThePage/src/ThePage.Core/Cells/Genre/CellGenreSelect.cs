using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class CellGenreSelect : MvxNotifyPropertyChanged, ICellBaseSelect<Genre>
    {
        #region Properties

        public Genre Item { get; set; }

        public bool IsSelected { get; set; }

        public string IconSelected => Constants.ICON_CHECK;

        #endregion

        #region Contructor

        public CellGenreSelect(Genre genre, bool isSelected = false)
        {
            Item = genre;
            IsSelected = genre != null && isSelected;
        }

        #endregion
    }
}