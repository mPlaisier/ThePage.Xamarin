using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
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

        IMvxCommand<Author> _itemClickCommand;
        public IMvxCommand<Author> ItemClickCommand => _itemClickCommand ??= new MvxCommand<Author>(async (item) =>
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
            await Refresh();

            await base.Initialize();
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            Authors = await _thePageService.GetAllAuthors();
        }

        #endregion
    }
}
