using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorViewModel : BaseViewModel, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        public override string Title => "Authors";

        public List<CellAuthor> Authors { get; set; }

        #endregion

        #region Constructor

        public AuthorViewModel(IMvxNavigationService navigation, IThePageService thePageService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
        }

        #endregion

        #region Commands

        IMvxCommand<CellAuthor> _itemClickCommand;
        public IMvxCommand<CellAuthor> ItemClickCommand => _itemClickCommand ??= new MvxCommand<CellAuthor>(async (item) =>
        {
            var result = await _navigation.Navigate<AuthorDetailViewModel, AuthorDetailParameter, bool>(new AuthorDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxCommand _addAuthorCommand;
        public IMvxCommand AddAuthorCommand => _addAuthorCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddAuthorViewModel, bool>();
            if (result)
                await Refresh();
        });

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AuthorViewModel)}");

            await base.Initialize();

            Refresh().Forget();
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var authors = await _thePageService.GetAllAuthors();
            Authors = AuthorBusinessLogic.AuthorsToCellAuthors(authors);

            IsLoading = false;
        }

        #endregion
    }
}
