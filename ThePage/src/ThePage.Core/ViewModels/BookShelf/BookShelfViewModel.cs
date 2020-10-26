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

        public override string LblTitle => "Bookshelves";

        public List<ApiBookShelf> BookShelves { get; internal set; }

        #endregion

        #region Constructor

        public BookShelfViewModel(IMvxNavigationService navigation, IThePageService thePageService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
        }

        #endregion

        #region Commands

        IMvxAsyncCommand<ApiBookShelf> _itemClickCommand;
        public IMvxAsyncCommand<ApiBookShelf> ItemClickCommand => _itemClickCommand ??= new MvxAsyncCommand<ApiBookShelf>(GoToBookShelfDetail);

        IMvxAsyncCommand _addBookShelfCommand;
        public IMvxAsyncCommand AddBookShelfCommand => _addBookShelfCommand ??= new MvxAsyncCommand(AddBookShelf);

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

        async Task AddBookShelf()
        {
            var result = await _navigation.Navigate<AddBookShelfViewModel, bool>();
            if (result)
                await Refresh();
        }

        async Task GoToBookShelfDetail(ApiBookShelf bookshelf)
        {
            var result = await _navigation.Navigate<BookShelfDetailViewModel, ApiBookShelf, bool>(bookshelf);
            if (result)
                await Refresh();
        }

        #endregion
    }
}