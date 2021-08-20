using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.Cells;

namespace ThePage.Core
{
    public abstract class BaseBookDetailScreenManager : MvxNotifyPropertyChanged, IBaseBookDetailScreenManager
    {
        protected readonly IMvxNavigationService _navigation;
        protected readonly IUserInteraction _userInteraction;
        protected readonly IDevice _device;

        #region Properties

        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            internal set
            {
                if (value.Equals(_isLoading))
                    return;

                _isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }

        public MvxObservableCollection<ICellBook> Items { get; internal set; }
            = new MvxObservableCollection<ICellBook>();

        #endregion

        #region Constructor

        protected BaseBookDetailScreenManager(IMvxNavigationService navigationService,
                                           IUserInteraction userInteraction,
                                           IDevice device)
        {
            _navigation = navigationService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region public abstract

        public abstract void CreateCellBooks(BookDetail bookDetail);

        public abstract Task SaveBook();

        public abstract Task FetchData();

        #endregion

        #region Protected

        protected void UpdateValidation()
        {
            var lstInput = Items.OfType<CellBaseBookInput>();
            var isValid = lstInput.All(x => x.IsValid);

            Items.ForEachType<ICellBook, CellBookButton>(x => x.IsValid = isValid);
            RaisePropertyChanged(nameof(Items));
        }

        protected virtual async Task AddGenre()
        {
            var selectedGenres = Items.OfType<CellBookGenreItem>().Select(i => i.Genre).ToList();
            var genres = await _navigation.Navigate<SelectGenreViewModel, SelectedGenreParameters, List<Genre>>(
                                                    new SelectedGenreParameters(selectedGenres));

            if (genres.IsNotNull())
            {
                //Remove all old genres:
                Items.RemoveItems(Items.OfType<CellBookGenreItem>());

                var genreItems = new List<CellBookGenreItem>();
                genres.ForEach(x => genreItems.Add(new CellBookGenreItem(x, RemoveGenre, true)));

                var index = Items.FindIndex(x => x is CellBookAddGenre);
                Items.InsertRange(index, genreItems);

                await RaisePropertyChanged(nameof(Items));
            }
        }

        protected void RemoveGenre(CellBookGenreItem obj)
        {
            Items.Remove(obj);
            RaisePropertyChanged(nameof(Items));
        }

        #endregion
    }
}