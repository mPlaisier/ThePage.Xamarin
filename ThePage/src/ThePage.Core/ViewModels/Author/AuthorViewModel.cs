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
    public class AuthorViewModel : BaseViewModel, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;

        int _currentPage;
        bool _hasNextPage;
        bool _isLoadingNextPage;

        #region Properties

        public override string LblTitle => "Authors";

        public List<ApiAuthor> Authors { get; set; }

        #endregion

        #region Constructor

        public AuthorViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region Commands

        IMvxCommand<ApiAuthor> _itemClickCommand;
        public IMvxCommand<ApiAuthor> ItemClickCommand => _itemClickCommand ??= new MvxCommand<ApiAuthor>(async (item) =>
        {
            var result = await _navigation.Navigate<AuthorDetailViewModel, AuthorDetailParameter, bool>(new AuthorDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxCommand _addAuthorCommand;
        public IMvxCommand AddAuthorCommand => _addAuthorCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddAuthorViewModel, ApiAuthor>();
            if (result != null)
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

        public async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiAuthorResponse = await _thePageService.GetNextAuthors(_currentPage + 1);
                Authors.AddRange(apiAuthorResponse.Docs);

                _currentPage = apiAuthorResponse.Page;
                _hasNextPage = apiAuthorResponse.HasNextPage;

                _isLoadingNextPage = false;
                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
            }
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var authors = await _thePageService.GetAllAuthors();
            Authors = authors.Docs;

            _currentPage = authors.Page;
            _hasNextPage = authors.HasNextPage;
            IsLoading = false;
        }

        #endregion
    }
}
