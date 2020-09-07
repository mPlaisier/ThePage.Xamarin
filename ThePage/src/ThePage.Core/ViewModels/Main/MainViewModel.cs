using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace ThePage.Core.ViewModels.Main
{
    public class MainViewModel : BaseViewModel
    {
        public enum EMenu
        {
            Books,
            Authors,
            Genre,
            Debug
        }

        readonly IMvxNavigationService _navigationService;

        #region Properties

        public override string LblTitle => "ThePage";

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
            Analytics.TrackEvent($"Initialize {nameof(MainViewModel)}");

            CreateMenuItems();

            return base.Initialize();
        }

        #endregion

        #region Private

        private void CreateMenuItems()
        {
#if DEBUG
            Items = new List<CellMenu>
            {
               new CellMenuHeader("ThePage"),
               new CellMenuItem("Books",EMenu.Books),
               new CellMenuItem("Authors",EMenu.Authors),
               new CellMenuItem("Genres", EMenu.Genre),
               new CellMenuHeader("Other"),
               new CellMenuItem("Debug",EMenu.Debug)
            };
#else
            Items = new List<CellMenu>
            {
                new CellMenuItem("Books",EMenu.Books),
                new CellMenuItem("Authors",EMenu.Authors),
                new CellMenuItem("Genres", EMenu.Genre)
            };
#endif
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
                    case EMenu.Genre:
                        _navigationService.Navigate<GenreViewModel>();
                        break;
                    case EMenu.Debug:
                        _navigationService.Navigate<DebugViewModel>();
                        break;
                    default:
                        throw new NotSupportedException("Unknown Menu item");
                }
            }
        }

        #endregion
    }
}