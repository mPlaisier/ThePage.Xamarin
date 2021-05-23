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
            Book,
            Author,
            Genre,
            BookShelf,
            Debug
        }

        readonly IMvxNavigationService _navigationService;

        #region Properties

        public override string LblTitle => "ThePage";

        public List<CellMenu> Items { get; set; }

        #endregion

        #region Commands

        private MvxCommand<CellMenu> _itemClickCommand;
        public MvxCommand<CellMenu> ItemClickCommand => _itemClickCommand ??= new MvxCommand<CellMenu>(OnItemClick);

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
               new CellMenuItem("Books",EMenu.Book),
               new CellMenuItem("Authors",EMenu.Author),
               new CellMenuItem("Genres", EMenu.Genre),
               new CellMenuItem("Bookshelves", EMenu.BookShelf),
               new CellMenuHeader("Other"),
               new CellMenuItem("Debug",EMenu.Debug)
            };
#else
            Items = new List<CellMenu>
            {
                new CellMenuItem("Books",EMenu.Book),
                new CellMenuItem("Authors",EMenu.Author),
                new CellMenuItem("Genres", EMenu.Genre),
                new CellMenuItem("Bookshelves", EMenu.BookShelf)
            };
#endif
        }

        private void OnItemClick(CellMenu cell)
        {
            if (cell is CellMenuItem item)
            {
                switch (item.Menu)
                {
                    case EMenu.Book:
                        _navigationService.Navigate<BookViewModel>();
                        break;
                    case EMenu.Author:
                        _navigationService.Navigate<AuthorViewModel>();
                        break;
                    case EMenu.Genre:
                        _navigationService.Navigate<GenreViewModel>();
                        break;
                    case EMenu.BookShelf:
                        _navigationService.Navigate<BookShelfViewModel>();
                        break;
                    case EMenu.Debug:
                        _navigationService.Navigate<DebugViewModel>();
                        break;
                    default:
                        throw new NotSupportedException($"{nameof(MainViewModel)}: OnItemClick - Unknown Menu item: {item.Menu}");
                }
            }
        }

        #endregion
    }
}