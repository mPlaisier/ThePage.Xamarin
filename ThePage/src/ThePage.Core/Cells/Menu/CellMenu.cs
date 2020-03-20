using System;
using static ThePage.Core.ViewModels.Main.MainViewModel;

namespace ThePage.Core
{
    public interface ICellMenu { }

    public class CellMenu : ICellMenu
    {

    }

    public class CellMenuHeader : CellMenu
    {
        #region Properties

        public string Title { get; }

        #endregion

        #region Constructor

        public CellMenuHeader(string title)
        {
            Title = title;
        }

        #endregion
    }

    public class CellMenuItem : CellMenu
    {
        #region Properties

        public string Title { get; }

        public EMenu Menu { get; }

        #endregion

        #region Constructor

        public CellMenuItem(string title, EMenu eMenu)
        {
            Title = title;
            Menu = eMenu;
        }

        #endregion
    }
}
