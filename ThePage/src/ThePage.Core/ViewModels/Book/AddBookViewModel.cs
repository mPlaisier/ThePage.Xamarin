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

        #region Properties

        public override string Title => "Add book";

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

        public string LblBtn => "Add Book";

        #endregion

        #region Commands

        IMvxCommand _addbookCommand;
        public IMvxCommand AddBookCommand => _addbookCommand ??= new MvxCommand(async () =>
        {
            await AddBook();
        });

        #endregion

        #region Constructor

        public AddBookViewModel(IMvxNavigationService navigation)
        {
            _navigation = navigation;
        }

        #endregion

        #region Private

        async Task AddBook()
        {
            var book = new Book(TxtName, TxtAuthor);
            book = BookManager.AddBook(book, CancellationToken.None).Result;

            if (book != null)
            {
                await _navigation.Close(this, true);
            }
        }

        #endregion

    }
}
