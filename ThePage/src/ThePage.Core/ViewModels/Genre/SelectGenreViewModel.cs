using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class SelectedGenreParameters
    {
        #region Properties

        public List<ApiGenre> SelectedGenres { get; }

        #endregion

        #region Constructor

        public SelectedGenreParameters(List<ApiGenre> selectedGenres)
        {
            SelectedGenres = selectedGenres;
        }

        #endregion
    }

    public class SelectGenreViewModel : BaseSelectMultipleItemsViewModel<SelectedGenreParameters, List<ApiGenre>, CellGenreSelect, ApiGenre>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;

        #region Properties

        public override string LblTitle => "Select Genre";

        public override MvxObservableCollection<CellGenreSelect> Items { get; set; }

        public override List<ApiGenre> SelectedItems { get; internal set; }

        #endregion

        #region Commands

        IMvxCommand<CellGenreSelect> _commandSelectItem;
        public override IMvxCommand<CellGenreSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellGenreSelect>(HandleGenreClick);

        IMvxCommand _commandAddItem;
        public override IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddGenreViewModel, string>();
            if (result != null)
                await LoadData(result);
        });

        IMvxCommand _commandConfirm;
        public override IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public SelectGenreViewModel(IMvxNavigationService navigationService, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _navigation = navigationService;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(SelectedGenreParameters parameter)
        {
            SelectedItems = parameter.SelectedGenres;
        }

        public override Task Initialize()
        {
            LoadData().Forget();

            return base.Initialize();
        }

        #endregion

        #region Public

        public override async Task LoadData(string id = null)
        {
            IsLoading = true;

            var genres = await _thePageService.GetAllGenres();

            //Add new created genre to Selected list
            if (id != null)
            {
                var newGenre = await _thePageService.GetGenre(id);
                if (newGenre != null)
                    SelectedItems.Add(newGenre);
            }

            Items = new MvxObservableCollection<CellGenreSelect>();
            genres.Docs.ForEach(x => Items.Add(
                new CellGenreSelect(x, SelectedItems.Contains(x))));

            _currentPage = genres.Page;
            _hasNextPage = genres.HasNextPage;
            IsLoading = false;
        }

        public override async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiGenreResponse = await _thePageService.GetNextGenres(_currentPage + 1);
                apiGenreResponse.Docs.ForEach(x => Items.Add(
                    new CellGenreSelect(x, SelectedItems.Contains(x))));

                _currentPage = apiGenreResponse.Page;
                _hasNextPage = apiGenreResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
            }
        }

        #endregion

        #region Private

        void HandleGenreClick(CellGenreSelect cellGenre)
        {
            if (cellGenre.IsSelected)
            {
                SelectedItems.Remove(cellGenre.Item);
                cellGenre.IsSelected = false;
            }
            else
            {
                SelectedItems.Add(cellGenre.Item);
                cellGenre.IsSelected = true;
            }
        }

        void HandleConfirm()
        {
            _navigation.Close(this, SelectedItems);
        }

        #endregion
    }
}