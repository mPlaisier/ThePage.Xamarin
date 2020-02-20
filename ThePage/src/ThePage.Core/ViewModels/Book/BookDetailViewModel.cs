using System;
using System.Threading;
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

        public Book Book { get; }

        #endregion

        #region Constructor

        public BookDetailParameter(Book book)
        {
            Book = book;
        }

        #endregion
    }
    public class BookDetailViewModel : BaseViewModel<BookDetailParameter, bool>
    {
        readonly IMvxNavigationService _navigation;

        #region Properties

        public override string Title => "Book Detail";

        public Book Book { get; internal set; }

        public string LblName => "Title:";

        public string LblAuthor => "Author:";

        string _txtName;
        public string TxtName
        {
            get => _txtName;
            set
            {
                SetProperty(ref _txtName, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        string _txtAuthor;
        public string TxtAuthor
        {
            get => _txtAuthor;
            set
            {
                SetProperty(ref _txtAuthor, value);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => !string.IsNullOrEmpty(TxtName) && !string.IsNullOrEmpty(TxtAuthor);

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
        });

        //DeleteBookCommand
        IMvxCommand _deleteBookCommand;
        public IMvxCommand DeleteBookCommand => _deleteBookCommand ??= new MvxCommand(async () =>
        {
            await DeleteBook();
        });

        IMvxCommand _updateBookCommand;
        public IMvxCommand UpdateBookCommand => _updateBookCommand ??= new MvxCommand(async () =>
        {
            await UpdateBook();
        });

        #endregion

        #region Constructor

        public BookDetailViewModel(IMvxNavigationService navigation)
        {
            _navigation = navigation;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(BookDetailParameter parameter)
        {
            Book = parameter.Book;

            TxtName = Book.Title;
            TxtAuthor = Book.Author;
        }

        #endregion

        #region Private

        async Task UpdateBook()
        {
            Book.Title = TxtName;
            Book.Author = TxtAuthor;

            var book = BookManager.UpdateBook(Book, CancellationToken.None).Result;

            if (book != null)
                await _navigation.Close(this, true);
        }

        async Task DeleteBook()
        {
            var result = BookManager.DeleteBook(Book, CancellationToken.None).Result;

            if (result)
                await _navigation.Close(this, true);
        }

        #endregion
    }
}
