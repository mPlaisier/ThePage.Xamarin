using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorViewModel : BaseViewModel
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        List<AuthorCell> _authors;
        public List<AuthorCell> Authors
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }

        public override string Title => "Authors";

        #endregion

        #region Constructor

        public AuthorViewModel(IMvxNavigationService navigation, IThePageService thePageService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
        }

        #endregion

        #region Commands

        IMvxCommand<AuthorCell> _itemClickCommand;
        public IMvxCommand<AuthorCell> ItemClickCommand => _itemClickCommand ??= new MvxCommand<AuthorCell>(async (item) =>
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
            await base.Initialize();

            Refresh().Forget();
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var authors = await _thePageService.GetAllAuthors();
            Authors = AuthorBusinessLogic.AuthorToAuthorCell(authors);

            IsLoading = false;
        }

        #endregion
    }
}
