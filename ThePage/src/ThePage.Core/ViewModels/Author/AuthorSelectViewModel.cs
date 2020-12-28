using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class AuthorSelectParameter
    {
        #region Properties

        public ApiAuthor SelectedAuthor { get; }

        #endregion

        #region Constructor

        public AuthorSelectParameter(ApiAuthor selectedAuthor)
        {
            SelectedAuthor = selectedAuthor;
        }

        #endregion
    }

    public class AuthorSelectViewModel : BaseSelectSingleItemViewModel<AuthorSelectParameter, ApiAuthor, CellAuthorSelect>
    {
        readonly IThePageService _thePageService;
        readonly IMvxNavigationService _navigationService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Select Author";

        public override MvxObservableCollection<CellAuthorSelect> Items { get; set; }

        public override ApiAuthor SelectedItem { get; internal set; }

        #endregion

        #region Commands

        IMvxCommand<CellAuthorSelect> _commandSelectItem;
        public override IMvxCommand<CellAuthorSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellAuthorSelect>((cell) =>
        {
            _navigationService.Close(this, cell.Item);
        });

        IMvxCommand _commandAddItem;
        public override IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigationService.Navigate<AddAuthorViewModel, ApiAuthor>();
            if (result != null)
                await _navigationService.Close(this, result);

        });

        #endregion

        #region Constructor

        public AuthorSelectViewModel(IThePageService thePageService,
                                     IMvxNavigationService navigationService,
                                     IUserInteraction userInteraction,
                                     IDevice device)
        {
            _thePageService = thePageService;
            _navigationService = navigationService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(AuthorSelectParameter parameter)
        {
            SelectedItem = parameter.SelectedAuthor;
        }

        public override Task Initialize()
        {
            Refresh().Forget();

            return base.Initialize();
        }

        #endregion

        #region Public

        public override async Task Refresh()
        {
            IsLoading = true;

            var apiAuthorResponse = await _thePageService.GetAllAuthors();

            Items = new MvxObservableCollection<CellAuthorSelect>();
            apiAuthorResponse.Docs.ForEach(x => Items.Add(
                new CellAuthorSelect(x, x == SelectedItem)));

            _currentPage = apiAuthorResponse.Page;
            _hasNextPage = apiAuthorResponse.HasNextPage;
            IsLoading = false;
        }

        public override async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                ApiAuthorResponse apiAuthorResponse = _isSearching
                    ? await _thePageService.SearchAuthors(_search, _currentPage + 1)
                    : await _thePageService.GetNextAuthors(_currentPage + 1);

                apiAuthorResponse.Docs.ForEach(x => Items.Add(
                    new CellAuthorSelect(x, x == SelectedItem)));

                _currentPage = apiAuthorResponse.Page;
                _hasNextPage = apiAuthorResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
            }
        }

        public override async Task Search(string search)
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();

            if (_search != null && _search.Equals(search))
                return;

            IsLoading = true;
            _search = search;
            _isSearching = true;

            var apiAuthorResponse = await _thePageService.SearchAuthors(search, null);

            Items = new MvxObservableCollection<CellAuthorSelect>();
            apiAuthorResponse.Docs.ForEach(x => Items.Add(
                new CellAuthorSelect(x, x == SelectedItem)));

            _currentPage = apiAuthorResponse.Page;
            _hasNextPage = apiAuthorResponse.HasNextPage;

            IsLoading = false;
        }

        public override void StopSearch()
        {
            if (_isSearching)
            {
                _isSearching = false;
                _search = null;
                Refresh().Forget();
            }
        }

        #endregion
    }
}