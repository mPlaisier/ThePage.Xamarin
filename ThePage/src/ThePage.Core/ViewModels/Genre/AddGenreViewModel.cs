using System.Threading.Tasks;
using CBP.Extensions;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using PropertyChanged;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddGenreViewModel : BaseViewModelResult<string>
    {
        readonly IMvxNavigationService _navigation;
        readonly IGenreService _genreService;

        #region Properties

        public override string LblTitle => "New Genre";

        public string LblName => "Name:";

        [AlsoNotifyFor(nameof(IsValid))]
        public string TxtName { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(TxtName);

        public string LblBtn => "Add genre";

        #endregion

        #region Commands

        IMvxAsyncCommand _addGenreCommand;
        public IMvxAsyncCommand AddGenreCommand => _addGenreCommand ??= new MvxAsyncCommand(AddGenre);

        #endregion

        #region Constructor

        public AddGenreViewModel(IMvxNavigationService navigation, IGenreService genreService)
        {
            _navigation = navigation;
            _genreService = genreService;
        }

        #endregion

        #region LifeCycle

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddGenreViewModel)}");

            return base.Initialize();
        }

        #endregion

        #region Private

        async Task AddGenre()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _genreService.AddGenre(TxtName);
            if (result.IsNotNull())
                await _navigation.Close(this, result.Id);
            else
                IsLoading = false;
        }

        #endregion
    }
}