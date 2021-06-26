using System.Threading.Tasks;
using CBP.Extensions;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using PropertyChanged;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddAuthorViewModel : BaseViewModel<Author, Author>
    {
        readonly IMvxNavigationService _navigation;
        readonly IDevice _device;
        readonly IAuthorService _authorService;

        string _olkey;

        #region Properties

        public override string LblTitle => "New Author";

        public string LblName => "Name:";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtName { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblBtn => "Add author";

        #endregion

        #region Commands

        IMvxAsyncCommand _addAuthorCommand;
        public IMvxAsyncCommand AddAuthorCommand => _addAuthorCommand ??= new MvxAsyncCommand(async () =>
        {
            _device.HideKeyboard();
            await AddAuthor();
        });

        #endregion

        #region Constructor

        public AddAuthorViewModel(IMvxNavigationService navigation, IAuthorService authorService, IDevice device)
        {
            _navigation = navigation;
            _authorService = authorService;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(Author parameter)
        {
            _olkey = parameter.Olkey;
            TxtName = parameter.Name;
        }

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddAuthorViewModel)}");

            return base.Initialize();
        }

        #endregion

        #region Private

        async Task AddAuthor()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _authorService.AddAuthor(TxtName);
            if (result.IsNotNull())
                await _navigation.Close(this, result);
            else
                IsLoading = false;
        }

        #endregion
    }
}