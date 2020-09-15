using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookShelfViewModel : BaseViewModel, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        public override string LblTitle => "Bookshelfs";

        public List<ApiBookShelf> BookShelves { get; set; }

        #endregion

        #region Constructor

        public BookShelfViewModel(IMvxNavigationService navigation, IThePageService thePageService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
        }

        #endregion

        #region Commands

        IMvxCommand<ApiBookShelf> _itemClickCommand;
        public IMvxCommand<ApiBookShelf> ItemClickCommand => _itemClickCommand ??= new MvxCommand<ApiBookShelf>((item) =>
       {
           //TODO in #119
       });

        IMvxCommand _addBookShelfCommand;
        public IMvxCommand AddBookShelfCommand => _addBookShelfCommand ??= new MvxCommand(() =>
       {
           //TODO in #121
       });

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookShelfViewModel)}");

            await base.Initialize();

            Refresh().Forget();
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var apiBookShelfResponse = await _thePageService.GetAllBookShelves();
            BookShelves = apiBookShelfResponse.Docs;

            IsLoading = false;
        }

        #endregion
    }
}
