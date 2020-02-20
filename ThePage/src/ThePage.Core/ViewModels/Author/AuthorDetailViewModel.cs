using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorDetailParameter
    {
        #region Properties

        public Author Author { get; }

        #endregion

        #region Constructor

        public AuthorDetailParameter(Author author)
        {
            Author = author;
        }

        #endregion
    }

    public class AuthorDetailViewModel : BaseViewModel<AuthorDetailParameter, bool>
    {
        readonly IMvxNavigationService _navigation;

        #region Properties

        public override string Title => "Author Detail";

        public Author Author { get; internal set; }

        public string LblName => "Name:";

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

        public bool IsValid => !string.IsNullOrEmpty(TxtName);

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
            await DeleteAuthor();
        });

        IMvxCommand _updateBookCommand;
        public IMvxCommand UpdateBookCommand => _updateBookCommand ??= new MvxCommand(async () =>
        {
            await UpdateAuthor();
        });

        #endregion

        #region Constructor

        public AuthorDetailViewModel(IMvxNavigationService navigation)
        {
            _navigation = navigation;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(AuthorDetailParameter parameter)
        {
            Author = parameter.Author;

            TxtName = Author.Name;
        }

        #endregion

        #region Private

        async Task UpdateAuthor()
        {
            Author.Name = TxtName;

            var author = AuthorManager.UpdateAuthor(Author, CancellationToken.None).Result;

            if (author != null)
                await _navigation.Close(this, true);
        }

        async Task DeleteAuthor()
        {
            var result = AuthorManager.DeleteAuthor(Author, CancellationToken.None).Result;

            if (result)
                await _navigation.Close(this, true);
        }

        #endregion
    }
}
