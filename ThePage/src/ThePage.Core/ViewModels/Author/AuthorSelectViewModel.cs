using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
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

        #region Properties

        public override string LblTitle => "Select Author";

        public override List<CellAuthorSelect> Items { get; set; }

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

        public AuthorSelectViewModel(IThePageService thePageService, IMvxNavigationService navigationService, IUserInteraction userInteraction)
        {
            _thePageService = thePageService;
            _navigationService = navigationService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(AuthorSelectParameter parameter)
        {
            SelectedItem = parameter.SelectedAuthor;
        }

        public override Task Initialize()
        {
            LoadData().Forget();

            return base.Initialize();
        }

        #endregion

        #region Public

        public override async Task LoadData()
        {
            IsLoading = true;

            var authors = await _thePageService.GetAllAuthors();

            Items = new List<CellAuthorSelect>();
            authors.Docs.ForEach(x => Items.Add(
                new CellAuthorSelect(x, x == SelectedItem)));

            _currentPage = authors.Page;
            _hasNextPage = authors.HasNextPage;
            IsLoading = false;
        }

        public override async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiAuthorResponse = await _thePageService.GetNextAuthors(_currentPage + 1);
                apiAuthorResponse.Docs.ForEach(x => Items.Add(
                    new CellAuthorSelect(x, x == SelectedItem)));

                _currentPage = apiAuthorResponse.Page;
                _hasNextPage = apiAuthorResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
            }
        }

        #endregion
    }
}