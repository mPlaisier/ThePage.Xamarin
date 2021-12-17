using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorSelectParameter
    {
        #region Properties

        public Author SelectedAuthor { get; }

        #endregion

        #region Constructor

        public AuthorSelectParameter(Author selectedAuthor = null)
        {
            SelectedAuthor = selectedAuthor;
        }

        #endregion
    }

    public class AuthorSelectViewModel : BaseListViewModel<AuthorSelectParameter, Author>,
                                         IBaseSelectSingleItemViewModel<Author, CellAuthorSelect>
    {
        readonly IMvxNavigationService _navigationService;
        readonly IAuthorService _authorService;

        #region Properties

        public override string LblTitle => "Select Author";

        public MvxObservableCollection<CellAuthorSelect> Items { get; set; } = new MvxObservableCollection<CellAuthorSelect>();

        public Author SelectedItem { get; private set; }

        #endregion

        #region Commands

        IMvxCommand<CellAuthorSelect> _commandSelectItem;
        public IMvxCommand<CellAuthorSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellAuthorSelect>((cell) =>
        {
            _navigationService.Close(this, cell.Item);
        });

        IMvxAsyncCommand _commandAddItem;
        public IMvxAsyncCommand CommandAddItem => _commandAddItem ??= new MvxAsyncCommand(async () =>
        {
            var result = await _navigationService.Navigate<AddAuthorViewModel, Author>();
            if (result != null)
                await _navigationService.Close(this, result);

        });

        #endregion

        #region Constructor

        public AuthorSelectViewModel(IMvxNavigationService navigationService,
                                     IAuthorService authorService)
        {
            _navigationService = navigationService;
            _authorService = authorService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(AuthorSelectParameter parameter)
        {
            SelectedItem = parameter.SelectedAuthor;
        }

        public override async Task Initialize()
        {
            await Refresh();

            await base.Initialize();
        }

        #endregion

        #region Public

        public async Task Refresh()
        {
            IsLoading = true;

            var authors = await _authorService.GetAuthors();
            var cells = authors.Select(x => new CellAuthorSelect(x, x == SelectedItem));

            if (cells.IsNotNull())
                Items = new MvxObservableCollection<CellAuthorSelect>(cells);

            IsLoading = false;
        }

        public override async Task LoadNextPage()
        {
            if (!IsLoading)
            {
                var authors = await _authorService.LoadNextAuthors();
                var cells = authors.Select(x => new CellAuthorSelect(x, x == SelectedItem));
                Items.AddRange(cells);
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
            var cells = authors.Select(x => new CellAuthorSelect(x, x == SelectedItem));
            Items = new MvxObservableCollection<CellAuthorSelect>(cells);

            IsLoading = false;
        }

        public override async Task StopSearch()
        {
            if (_authorService.IsSearching)
                await Refresh().ConfigureAwait(false);
        }

        #endregion
    }
}