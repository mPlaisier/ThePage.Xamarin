using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddBookViewModel : BaseViewModelResult<bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        public override string Title => "Add book";

        public string LblTitle => "Title:";

        public string LblAuthor => "Author:";

        string _txtTitle;
        public string TxtTitle
        {
            get => _txtTitle;
            set
            {
                SetProperty(ref _txtTitle, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        List<AuthorCell> _authors;
        public List<AuthorCell> Authors
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }

        AuthorCell _selectedAuthor;
        public AuthorCell SelectedAuthor
        {
            get => _selectedAuthor;
            set => SetProperty(ref _selectedAuthor, value);
        }

        public bool IsValid => !string.IsNullOrEmpty(TxtTitle) && SelectedAuthor != null;

        public string LblBtn => "Add Book";

        #endregion

        #region Commands

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(() => AddBook().Forget());

        IMvxCommand<AuthorCell> _itemSelectedCommand;
        public IMvxCommand<AuthorCell> ItemSelectedCommand => _itemSelectedCommand ??= new MvxCommand<AuthorCell>((authorCell) =>
        {
            SelectedAuthor = authorCell;
        });

        #endregion

        #region Constructor

        public AddBookViewModel(IMvxNavigationService navigation, IThePageService thePageService)
        {
            _navigation = navigation;
            _thePageService = thePageService;
        }

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            await base.Initialize();

            FetchAuthors().Forget();
        }

        #endregion

        #region Private

        async Task AddBook()
        {
            if (IsLoading)
                return;

            var book = new Book(TxtTitle, SelectedAuthor.Id);
            var result = await _thePageService.AddBook(book);

            if (result)
                await _navigation.Close(this, true);

            IsLoading = false;
        }

        async Task FetchAuthors()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var authors = await _thePageService.GetAllAuthors();

            if (authors != null)
                Authors = AuthorBusinessLogic.AuthorsToAuthorCells(authors);

            IsLoading = false;
        }

        #endregion

    }
}
