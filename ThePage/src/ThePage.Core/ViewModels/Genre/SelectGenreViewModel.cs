using System.Collections.Generic;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

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

    public class SelectGenreViewModel : BaseListViewModel<SelectedGenreParameters, List<ApiGenre>>,
                                        IBaseSelectMultipleItemsViewModel<CellGenreSelect, ApiGenre>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Select Genre";

        public MvxObservableCollection<CellGenreSelect> Items { get; set; }

        public List<ApiGenre> SelectedItems { get; internal set; }

        #endregion

        #region Commands

        IMvxCommand<CellGenreSelect> _commandSelectItem;
        public IMvxCommand<CellGenreSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellGenreSelect>(HandleGenreClick);

        IMvxCommand _commandAddItem;
        public IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddGenreViewModel, string>();
            if (result != null)
                await Refresh(result);
        });

        IMvxCommand _commandConfirm;
        public IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public SelectGenreViewModel(IMvxNavigationService navigationService,
                                    IThePageService thePageService,
                                    IUserInteraction userInteraction,
                                    IDevice device)
        {
            _navigation = navigationService;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(SelectedGenreParameters parameter)
        {
            SelectedItems = parameter.SelectedGenres;
        }

        public override Task Initialize()
        {
            Refresh().Forget();

            return base.Initialize();
        }

        #endregion

        #region Public

        public override async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiGenreResponse = _isSearching
                    ? await _thePageService.SearchGenres(_search, _currentPage + 1)
                    : await _thePageService.GetNextGenres(_currentPage + 1);

                apiGenreResponse.Docs.ForEach(x => Items.Add(
                    new CellGenreSelect(x, SelectedItems.Contains(x))));

                _currentPage = apiGenreResponse.Page;
                _hasNextPage = apiGenreResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
            }
        }

        public override async Task Search(string search)
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();

            if (_search != null && _search.Equals(search))
                return;

            IsLoading = true;
            _search = search;
            _isSearching = true;

            var apiGenreResponse = await _thePageService.SearchGenres(search);

            Items = new MvxObservableCollection<CellGenreSelect>();
            apiGenreResponse.Docs.ForEach(x => Items.Add(
                new CellGenreSelect(x, SelectedItems.Contains(x))));

            _currentPage = apiGenreResponse.Page;
            _hasNextPage = apiGenreResponse.HasNextPage;

            IsLoading = false;
        }

        public override void StopSearch()
        {
            if (_isSearching)
            {
                _isSearching = false;
                _search = null;
                Refresh().Forget();
            }
        }

        #endregion

        #region Protected

        async Task Refresh(string item = null)
        {
            IsLoading = true;

            var genres = await _thePageService.GetAllGenres();

            //Add new created genre to Selected list
            if (item != null)
            {
                var newGenre = await _thePageService.GetGenre(item);
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