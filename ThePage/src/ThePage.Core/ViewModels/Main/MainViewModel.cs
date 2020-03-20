using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace ThePage.Core.ViewModels.Main
{
    public class MainViewModel : BaseViewModel
    {
        public enum EMenu
        {
            Books,
            Authors
        }

        IMvxNavigationService _navigationService;

        #region Properties

        public override string Title => "ThePage";

        public List<CellMenu> Items { get; set; }

        #endregion

        #region Commands

        private MvxCommand<CellMenu> _itemClickCommand;
        public MvxCommand<CellMenu> ItemClickCommand => _itemClickCommand = _itemClickCommand ?? new MvxCommand<CellMenu>(OnItemClick);


        #endregion

        #region Constructor

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

        #region LifeCycle

        public override Task Initialize()
        {
            CreateMenuItems();

            return base.Initialize();
        }

        #endregion

        #region Private

        private void CreateMenuItems()
        {
            Items = new List<CellMenu>
            {
               new CellMenuHeader("Header"),
               new CellMenuItem("Books",EMenu.Books),
               new CellMenuItem("Authors",EMenu.Authors)
            };
        }

        private void OnItemClick(CellMenu cell)
        {
            if (cell is CellMenuItem item)
            {
                switch (item.Menu)
                {
                    case EMenu.Books:
                        _navigationService.Navigate<BookViewModel>();
                        break;
                    case EMenu.Authors:
                        _navigationService.Navigate<AuthorViewModel>();
                        break;
                    default:
                        break;
                }
            }
        }


        #endregion
    }
}
