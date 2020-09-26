using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
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

    public class SelectGenreViewModel : BaseSelectMultipleItemsViewModel<SelectedGenreParameters, List<ApiGenre>, CellGenreSelect, ApiGenre>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;

        #region Properties

        public override string LblTitle => "Select Genre";

        public override List<CellGenreSelect> Items { get; set; }

        public override List<ApiGenre> SelectedItems { get; internal set; }

        #endregion

        #region Commands

        IMvxCommand<CellGenreSelect> _commandSelectItem;
        public override IMvxCommand<CellGenreSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellGenreSelect>(HandleGenreClick);

        IMvxCommand _commandAddItem;
        public override IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddGenreViewModel, bool>();
            if (result)
                await LoadData();
        });

        IMvxCommand _commandConfirm;
        public override IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public SelectGenreViewModel(IMvxNavigationService navigationService, IThePageService thePageService)
        {
            _navigation = navigationService;
            _thePageService = thePageService;
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

        public override async Task LoadData()
        {
            IsLoading = true;

            var genres = await _thePageService.GetAllGenres();

            IsLoading = false;

            Items = new List<CellGenreSelect>();
            genres.Docs.ForEach(x => Items.Add(new CellGenreSelect(x, SelectedItems.Contains(x))));
        }

        #endregion
    }
}