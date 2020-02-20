using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddAuthorViewModel : BaseViewModelResult<bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;

        #region Properties

        public override string Title => "Add book";

        public string LblName => "Title:";

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

        public string LblBtn => "Add Book";

        #endregion

        #region Commands

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(async () =>
        {
            await AddAuthor();
        });

        #endregion

        #region Constructor

        public AddAuthorViewModel(IMvxNavigationService navigation)
        {
            _navigation = navigation;
        }

        #endregion

        #region Private

        async Task AddAuthor()
        {
            var author = new Author(TxtName);
            author = AuthorManager.AddAuthor(author, CancellationToken.None).Result;

            if (author != null)
            {
                await _navigation.Close(this, true);
            }
        }

        #endregion

    }
}