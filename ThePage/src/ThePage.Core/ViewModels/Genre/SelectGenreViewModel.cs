using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class SelectedGenreParameters
    {
        #region Properties

        public List<Genre> SelectedGenres { get; }

        #endregion

        #region Constructor

        public SelectedGenreParameters(List<Genre> selectedGenres)
        {
            SelectedGenres = selectedGenres;
        }

        #endregion
    }

    public class SelectGenreViewModel : BaseListViewModel<SelectedGenreParameters, List<Genre>>,
                                        IBaseSelectMultipleItemsViewModel<CellGenreSelect, Genre>
    {
        readonly IMvxNavigationService _navigation;
        readonly IGenreService _genreService;

        #region Properties

        public override string LblTitle => "Select Genre";

        public MvxObservableCollection<CellGenreSelect> Items { get; set; } = new MvxObservableCollection<CellGenreSelect>();

        public List<Genre> SelectedItems { get; internal set; }

        #endregion

        #region Commands

        IMvxCommand<CellGenreSelect> _commandSelectItem;
        public IMvxCommand<CellGenreSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellGenreSelect>(HandleGenreClick);

        IMvxCommand _commandAddItem;
        public IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddGenreViewModel, string>();
            if (result != null)
            {
                //Add new created genre to Selected list
                await AddNewGenreToSelectedList(result);
                await Refresh();
            }
        });

        IMvxCommand _commandConfirm;
        public IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public SelectGenreViewModel(IMvxNavigationService navigationService,
                                    IGenreService genreService)
        {
            _navigation = navigationService;
            _genreService = genreService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(SelectedGenreParameters parameter)
        {
            SelectedItems = parameter.SelectedGenres;
        }

        public override async Task Initialize()
        {
            await Refresh();

            await base.Initialize();
        }

        #endregion

        #region Public

        public override async Task LoadNextPage()
        {
            if (!IsLoading)
            {
                var genres = await _genreService.LoadNextGenres();
                var cells = genres.Select(x => new CellGenreSelect(x, SelectedItems.Contains(x)));
                Items.AddRange(cells);
            }
        }

        public override async Task Search(string input)
        {
            if (IsLoading)
                return;

            var currentSearch = _genreService.SearchText;
            if (currentSearch != null && currentSearch.Equals(input))
                return;

            IsLoading = true;

            var genres = await _genreService.Search(input);
            var cells = genres.Select(x => new CellGenreSelect(x, SelectedItems.Contains(x)));
            Items = new MvxObservableCollection<CellGenreSelect>(cells);

            IsLoading = false;
        }

        public override async Task StopSearch()
        {
            if (_genreService.IsSearching)
                await Refresh().ConfigureAwait(false);
        }

        #endregion

        #region Protected

        async Task Refresh()
        {
            IsLoading = true;

            var genres = await _genreService.GetGenres();
            var cells = genres.Select(x => new CellGenreSelect(x, SelectedItems.Contains(x)));

            if (cells.IsNotNull())
                Items = new MvxObservableCollection<CellGenreSelect>(cells);

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

        async Task AddNewGenreToSelectedList(string id)
        {
            if (id != null)
            {
                var newGenre = await _genreService.GetGenre(id);
                if (newGenre != null)
                    SelectedItems.Add(newGenre);
            }
        }

        #endregion
    }
}