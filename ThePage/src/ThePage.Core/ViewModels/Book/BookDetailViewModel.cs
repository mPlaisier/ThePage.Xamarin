using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
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
    public class BookDetailViewModel : BaseViewModel<BookDetailParameter, bool>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;

        #region Properties

        public override string Title => "Book Detail";

        BookCell _bookCell;
        public BookCell Book
        {
            get => _bookCell;
            internal set => SetProperty(ref _bookCell, value);
        }

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

        List<Author> _authors;
        public List<Author> Authors
        {
            get => _authors;
            set => SetProperty(ref _authors, value);
        }

        Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set => SetProperty(ref _selectedAuthor, value);
        }

        public bool IsValid => !string.IsNullOrEmpty(TxtTitle) && SelectedAuthor != null;

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

        #endregion

        #region Constructor

        public BookDetailViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(BookDetailParameter parameter)
        {
            Book = parameter.Book;

            TxtTitle = Book.Title;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            FetchAuthors().Forget();
        }

        #endregion

        #region Private

        async Task UpdateBook()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            Book.Title = TxtTitle;
            Book.Author = SelectedAuthor;

            var result = await _thePageService.UpdateBook(BookBusinessLogic.BookCellToBook(Book));

            if (result != null)
            {
                Book = BookBusinessLogic.BookToBookCell(result, Authors);
                SelectedAuthor = Authors.FirstOrDefault(a => a.Id == Book.Author.Id);
            }

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
                    await _navigation.Close(this, true);
            }
        }

        async Task FetchAuthors()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            Authors = await _thePageService.GetAllAuthors();

            SelectedAuthor = Authors.FirstOrDefault(a => a.Id == Book.Author?.Id);

            IsLoading = false;
        }

        #endregion
    }
}
