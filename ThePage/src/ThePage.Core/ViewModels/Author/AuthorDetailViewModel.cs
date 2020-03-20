using System.ComponentModel;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AuthorDetailParameter
    {
        #region Properties

        public AuthorCell Author { get; }

        #endregion

        #region Constructor

        public AuthorDetailParameter(AuthorCell author)
        {
            Author = author;
        }

        #endregion
    }

    public class AuthorDetailViewModel : BaseViewModel<AuthorDetailParameter, bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;

        #region Properties

        public override string Title => "Author Detail";

        public AuthorCell Author { get; internal set; }

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

        public string LblUpdateBtn => "Update Author";

        public string LblDeleteBtn => "Delete Author";

        bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        #endregion

        #region Commands

        IMvxCommand _editAuthorCommand;
        public IMvxCommand EditAuthorCommand => _editAuthorCommand ??= new MvxCommand(() =>
        {
            IsEditing = !IsEditing;
        });

        IMvxCommand _deleteAuthorCommand;
        public IMvxCommand DeleteAuthorCommand => _deleteAuthorCommand ??= new MvxCommand(async () =>
        {
            //await DeleteAuthor();
        });

        IMvxCommand _updateAuthorCommand;
        public IMvxCommand UpdateAuthorCommand => _updateAuthorCommand ??= new MvxCommand(async () =>
        {
            //await UpdateAuthor();
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
            IsEditing = false;
        }

        #endregion

        #region Private

        //async Task UpdateAuthor()
        //{
        //    Author.Name = TxtName;

        //    var author = AuthorManager.UpdateAuthor(Author, CancellationToken.None).Result;

        //    if (author != null)
        //        await _navigation.Close(this, true);
        //}

        //async Task DeleteAuthor()
        //{
        //    var result = AuthorManager.DeleteAuthor(Author, CancellationToken.None).Result;

        //    if (result)
        //        await _navigation.Close(this, true);
        //}

        #endregion
    }
}
