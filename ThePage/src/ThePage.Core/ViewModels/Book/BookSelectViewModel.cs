using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class BookSelectViewModel : BaseSelectMultipleItemsViewModel<List<ApiBook>, List<ApiBook>, CellBookSelect, ApiBook>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;

        #region Properties

        public override string LblTitle => "Select Book";

        public override MvxObservableCollection<CellBookSelect> Items { get; set; }

        public override List<ApiBook> SelectedItems { get; internal set; }

        #endregion

        #region Commandds

        IMvxCommand<CellBookSelect> _commandSelectItem;
        public override IMvxCommand<CellBookSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellBookSelect>(HandleBookClick);

        IMvxCommand _commandAddItem;
        public override IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddBookViewModel, string>();
            if (result != null)
                await LoadData(result);
        });

        IMvxCommand _commandConfirm;
        public override IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public BookSelectViewModel(IMvxNavigationService navigationService, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _navigation = navigationService;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(List<ApiBook> parameter)
        {
            SelectedItems = parameter ?? new List<ApiBook>();
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

            var books = await _thePageService.GetAllBooks();

            //Add new created book to Selected list
            if (id != null)
                SelectedItems.Add(books.Docs.Where(x => x.Id.Equals(id)).FirstOrDefault());

            //Create select cells with the already selected books = true
            Items = new MvxObservableCollection<CellBookSelect>();
            books.Docs.ForEach(x => Items.Add(
                new CellBookSelect(x, SelectedItems.Contains(x))));

            _currentPage = books.Page;
            _hasNextPage = books.HasNextPage;
            IsLoading = false;
        }

        public override async Task LoadNextPage()
        {
            if (_hasNextPage && !_isLoadingNextPage && !IsLoading)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiBookResponse = await _thePageService.GetNextBooks(_currentPage + 1);
                apiBookResponse.Docs.ForEach(x => Items.Add(
                    new CellBookSelect(x, SelectedItems.Contains(x))));

                _currentPage = apiBookResponse.Page;
                _hasNextPage = apiBookResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
            }
        }

        #endregion

        #region Private

        void HandleBookClick(CellBookSelect cellBook)
        {
            if (cellBook.IsSelected)
            {
                SelectedItems.Remove(cellBook.Item);
                cellBook.IsSelected = false;
            }
            else
            {
                SelectedItems.Add(cellBook.Item);
                cellBook.IsSelected = true;
            }
        }

        void HandleConfirm()
        {
            _navigation.Close(this, SelectedItems);
        }

        #endregion
    }
}