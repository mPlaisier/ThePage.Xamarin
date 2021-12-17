using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorViewModel : BaseListViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IAuthorService _authorService;

        #region Properties

        public override string LblTitle => "Authors";

        public MvxObservableCollection<Author> Authors { get; private set; }

        #endregion

        #region Constructor

        public AuthorViewModel(IMvxNavigationService navigation, IAuthorService authorService)
        {
            _navigation = navigation;
            _authorService = authorService;
        }

        #endregion

        #region Commands

        IMvxAsyncCommand<Author> _itemClickCommand;
        public IMvxAsyncCommand<Author> ItemClickCommand => _itemClickCommand ??= new MvxAsyncCommand<Author>(async (item) =>
        {
            var result = await _navigation.Navigate<AuthorDetailViewModel, AuthorDetailParameter, bool>(new AuthorDetailParameter(item));
            if (result)
                await Refresh();

        });

        IMvxCommand _addAuthorCommand;
        public IMvxCommand AddAuthorCommand => _addAuthorCommand ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddAuthorViewModel, Author>();
            if (result != null)
                await Refresh();
        });

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AuthorViewModel)}");

            await base.Initialize();

            await Refresh();
        }

        #endregion

        #region Public

        public override async Task LoadNextPage()
        {
            if (!IsLoading)
            {
                var authors = await _authorService.LoadNextAuthors();
                Authors.AddRange(authors);
            }
        }

        public override async Task Search(string input)
        {
            if (IsLoading)
                return;

            var currentSearch = _authorService.SearchText;
            if (currentSearch != null && currentSearch.Equals(input))
                return;

            IsLoading = true;

            var authors = await _authorService.Search(input);
            Authors = new MvxObservableCollection<Author>(authors);

            IsLoading = false;
        }

        public override async Task StopSearch()
        {
            if (_authorService.IsSearching)
                await Refresh().ConfigureAwait(false);
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var authors = await _authorService.GetAuthors();
            Authors = new MvxObservableCollection<Author>(authors);

            IsLoading = false;
        }

        #endregion
    }
}