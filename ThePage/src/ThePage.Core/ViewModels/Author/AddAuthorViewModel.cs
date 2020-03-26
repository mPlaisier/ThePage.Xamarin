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
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;

        #region Properties

        public override string Title => "New Author";

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

        public string LblBtn => "Add author";

        #endregion

        #region Commands

        IMvxCommand _addAuthorCommand;
        public IMvxCommand AddAuthorCommand => _addAuthorCommand ??= new MvxCommand(async () =>
        {
            await AddAuthor();
        });

        #endregion

        #region Constructor

        public AddAuthorViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region Private

        async Task AddAuthor()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _thePageService.AddAuthor(new Author(TxtName));

            if (result)
            {
                _userInteraction.ToastMessage("Author added");
                await _navigation.Close(this, true);
            }
            else
            {
                _userInteraction.Alert("Failure adding author");
                IsLoading = false;
            }
        }

        #endregion
    }
}