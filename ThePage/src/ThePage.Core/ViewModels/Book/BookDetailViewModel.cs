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

        public BookCell Book { get; }

        #endregion

        #region Constructor

        public BookDetailParameter(BookCell book)
        {
            Book = book;
        }

        #endregion
    }
    public class BookDetailViewModel : BaseViewModel<BookDetailParameter, bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string Title => "Book Detail";

        public BookCell Book { get; internal set; }

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

        public List<Genre> AllGenres { get; set; }

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
                SelectedAuthor = Book.Author != null ? Authors.FirstOrDefault(a => a.Id == Book.Author.Id) : Authors[0];
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
            Book = parameter.Book;

            TxtTitle = Book.Title;
            Genres = new MvxObservableCollection<Genre>(Book.Genres);
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

            TxtTitle = TxtTitle.Trim();
            Book.Title = TxtTitle;
            Book.Author = SelectedAuthor;
            Book.Genres = Genres.ToList();

            var result = await _thePageService.UpdateBook(BookBusinessLogic.BookCellToBook(Book));

            if (result != null)
            {
                _userInteraction.ToastMessage("Book updated");
                Book = BookBusinessLogic.BookToBookCell(result, Authors, Genres.ToList());
                SelectedAuthor = Authors.FirstOrDefault(a => a.Id == Book.Author.Id);
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

                var result = await _thePageService.DeleteBook(BookBusinessLogic.BookCellToBook(Book));

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
            AllGenres = await _thePageService.GetAllGenres();

            SelectedAuthor = Authors.FirstOrDefault(a => a.Id == Book.Author?.Id);

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

            var genre = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, Genre>(new SelectedGenreParameters(AllGenres, Genres.ToList()));
            Genres.Add(genre);
        }

        #endregion
    }
}