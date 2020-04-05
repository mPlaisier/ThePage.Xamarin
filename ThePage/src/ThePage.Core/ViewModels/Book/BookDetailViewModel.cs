using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PropertyChanged;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookDetailParameter
    {
        #region Properties

        public CellBook Book { get; }

        #endregion

        #region Constructor

        public BookDetailParameter(CellBook book)
        {
            Book = book;
        }

        #endregion
    }
    public class BookDetailViewModel : BaseViewModel<BookDetailParameter, bool>, INotifyPropertyChanged
    {
        List<Genre> _allGenres;

        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string Title => "Book Detail";

        public CellBook BookCell { get; internal set; }

        public string LblTitle => "Title:";

        public string LblAuthor => "Author:";

        public string LblGenre => "Genres:";

        public string LblAddGenre => "Voeg genre toe";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtTitle { get; set; }

        public List<Author> Authors { get; set; }

        Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                SetProperty(ref _selectedAuthor, value);
                _device.HideKeyboard();
            }

        }

        public MvxObservableCollection<Genre> Genres { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtTitle) && SelectedAuthor != null;

        public string LblUpdateBtn => "Update Book";

        public string LblDeleteBtn => "Delete Book";

        bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        #endregion

        #region Commands

        //EditBookCommand
        IMvxCommand _editbookCommand;
        public IMvxCommand EditBookCommand => _editbookCommand ??= new MvxCommand(() =>
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;
            if (IsEditing)
            {
                SelectedAuthor = BookCell.Author != null ? Authors.FirstOrDefault(a => a.Id == BookCell.Author.Id) : Authors[0];
                RaisePropertyChanged(nameof(IsValid));
            }
        });

        IMvxCommand _deleteBookCommand;
        public IMvxCommand DeleteBookCommand => _deleteBookCommand ??= new MvxCommand(() => DeleteBook().Forget());

        IMvxCommand _updateBookCommand;
        public IMvxCommand UpdateBookCommand => _updateBookCommand ??= new MvxCommand(() => UpdateBook().Forget());

        IMvxCommand<Genre> _genreClickCommand;
        public IMvxCommand<Genre> GenreClickCommand => _genreClickCommand ??= new MvxCommand<Genre>((genre) => RemoveGenre(genre));

        IMvxCommand _addGenreCommand;
        public IMvxCommand AddGenreCommand => _addGenreCommand = _addGenreCommand ?? new MvxCommand(() => AddGenreAction().Forget());

        #endregion

        #region Constructor

        public BookDetailViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(BookDetailParameter parameter)
        {
            BookCell = parameter.Book;

            TxtTitle = BookCell.Book.Title;
            Genres = new MvxObservableCollection<Genre>(BookCell.Genres);
        }

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookDetailViewModel)}");

            await base.Initialize();

            FetchData().Forget();
        }

        #endregion

        #region Private

        async Task UpdateBook()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;

            //Get data for Book
            TxtTitle = TxtTitle.Trim();
            BookCell.Book.Title = TxtTitle;
            BookCell.Book.Author = SelectedAuthor.Id;
            BookCell.Book.Genres = Genres.GetIdStrings();

            var result = await _thePageService.UpdateBook(BookCell.Book);

            if (result != null)
            {
                //Update view
                BookCell.Genres = Genres.ToList();
                BookCell.Author = SelectedAuthor;

                _userInteraction.ToastMessage("Book updated");
                BookCell = BookBusinessLogic.BookToCellBook(result, Authors, _allGenres.ToList());
                SelectedAuthor = Authors.FirstOrDefault(a => a.Id == BookCell.Author.Id);
            }
            else
                _userInteraction.Alert("Failure updating book");

            IsEditing = false;
            IsLoading = false;
        }

        async Task DeleteBook()
        {
            if (IsLoading)
                return;

            var answer = await _userInteraction.ConfirmAsync("Remove book?", "Confirm", "DELETE");
            if (answer)
            {
                IsLoading = true;

                var result = await _thePageService.DeleteBook(BookCell.Book);

                if (result)
                {
                    _userInteraction.ToastMessage("Book removed");
                    await _navigation.Close(this, true);
                }
                else
                {
                    _userInteraction.Alert("Failure removing book");
                    IsLoading = false;
                }

            }
        }

        async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            Authors = await _thePageService.GetAllAuthors();
            _allGenres = await _thePageService.GetAllGenres();

            SelectedAuthor = Authors.FirstOrDefault(a => a.Id == BookCell.Author?.Id);

            IsLoading = false;
        }

        void RemoveGenre(Genre genre)
        {
            if (IsEditing)
                Genres.Remove(genre);
        }

        async Task AddGenreAction()
        {
            if (!IsEditing)
                return;

            var genre = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, Genre>(new SelectedGenreParameters(_allGenres, Genres.ToList()));
            if (genre != null)
                Genres.Add(genre);
        }

        #endregion
    }
}