using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
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
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        List<Genre> _genres;

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

        List<Genre> _selectedGenres;
        public List<Genre> SelectedGenres
        {
            get => _selectedGenres;
            set => SetProperty(ref _selectedGenres, value);
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtTitle) && SelectedAuthor != null;

        public string LblBtn => "Add Book";

        #endregion

        #region Commands

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(() => AddBook().Forget());

        IMvxCommand<AuthorCell> _itemSelectedCommand;
        public IMvxCommand<AuthorCell> ItemSelectedCommand => _itemSelectedCommand ??= new MvxCommand<AuthorCell>((authorCell) =>
        {
            _device.HideKeyboard();
            SelectedAuthor = authorCell;
        });

        #endregion

        #region Constructor

        public AddBookViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddBookViewModel)}");

            await base.Initialize();

            FetchData().Forget();
        }

        #endregion

        #region Private

        async Task AddBook()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();

            var book = new Book(TxtTitle.Trim(), SelectedAuthor.Id, SelectedGenres.GetIdAsStringList());
            var result = await _thePageService.AddBook(book);

            if (result)
            {
                _userInteraction.ToastMessage("Book added");
                await _navigation.Close(this, true);
            }
            else
            {
                _userInteraction.Alert("Failure adding book");
                IsLoading = false;
            }
        }

        async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var authors = await _thePageService.GetAllAuthors();
            _genres = await _thePageService.GetAllGenres();

            if (authors != null)
                Authors = AuthorBusinessLogic.AuthorsToAuthorCells(authors);
            else
            {
                _userInteraction.Alert("Error retrieving data from Server", null, "Error");
                await _navigation.Close(this, true);
            }

            IsLoading = false;
        }

        #endregion

    }
}
