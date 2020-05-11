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

        public Author SelectedAuthor { get; }

        #endregion

        #region Constructor

        public AuthorSelectParameter(Author selectedAuthor)
        {
            SelectedAuthor = selectedAuthor;
        }

        #endregion
    }

    public class AuthorSelectViewModel : BaseSelectSingleItemViewModel<AuthorSelectParameter, Author, CellAuthorSelect>
    {
        readonly IThePageService _thePageService;
        readonly IMvxNavigationService _navigationService;

        #region Properties

        public override string Title => "Select Author";

        public override List<CellAuthorSelect> Items { get; set; }

        public override Author SelectedItem { get; internal set; }

        #endregion

        #region Commands

        IMvxCommand<CellAuthorSelect> _commandSelectItem;
        public override IMvxCommand<CellAuthorSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellAuthorSelect>((cell) =>
        {
            _navigationService.Close(this, cell.Item);
        });

        #endregion

        #region Constructor

        public AuthorSelectViewModel(IThePageService thePageService, IMvxNavigationService navigationService)
        {
            _thePageService = thePageService;
            _navigationService = navigationService;
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

            var list = new List<CellAuthorSelect>();
            authors.ForEach(x => list.Add(new CellAuthorSelect(x, x == SelectedItem)));

            IsLoading = false;
        }

        #endregion
    }
}