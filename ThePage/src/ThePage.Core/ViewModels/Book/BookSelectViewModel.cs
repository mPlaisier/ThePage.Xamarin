using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookSelectViewModel : BaseListViewModel<List<ApiBook>, List<ApiBook>>,
                                       IBaseSelectMultipleItemsViewModel<CellBookSelect, ApiBook>
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        #region Properties

        public override string LblTitle => "Select Book";

        public MvxObservableCollection<CellBookSelect> Items { get; set; }

        public List<ApiBook> SelectedItems { get; internal set; }

        #endregion

        #region Commandds

        IMvxCommand<CellBookSelect> _commandSelectItem;
        public IMvxCommand<CellBookSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellBookSelect>(HandleBookClick);

        IMvxCommand _commandAddItem;
        public IMvxCommand CommandAddItem => _commandAddItem ??= new MvxCommand(async () =>
        {
            var result = await _navigation.Navigate<AddBookViewModel, string>();
            if (result != null)
                await Refresh(result);
        });

        IMvxCommand _commandConfirm;
        public IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public BookSelectViewModel(IMvxNavigationService navigationService,
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

        public override void Prepare(List<ApiBook> parameter)
        {
            SelectedItems = parameter ?? new List<ApiBook>();
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

                var apiBookResponse = _isSearching
                    ? await _thePageService.SearchBooksTitle(_search, _currentPage + 1)
                    : await _thePageService.GetNextBooks(_currentPage + 1);

                apiBookResponse.Docs.ForEach(x => Items.Add(
                    new CellBookSelect(x, SelectedItems.Contains(x))));

                _currentPage = apiBookResponse.Page;
                _hasNextPage = apiBookResponse.HasNextPage;
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

            var apiBookResponse = await _thePageService.SearchBooksTitle(search);

            Items = new MvxObservableCollection<CellBookSelect>();
            apiBookResponse.Docs.ForEach(x => Items.Add(
                new CellBookSelect(x, SelectedItems.Contains(x))));

            _currentPage = apiBookResponse.Page;
            _hasNextPage = apiBookResponse.HasNextPage;

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

        #region Private

        async Task Refresh(string item = null)
        {
            IsLoading = true;

            var books = await _thePageService.GetAllBooks();

            //Add new created book to Selected list
            if (item != null)
            {
                var newBook = await _thePageService.GetBook(item);
                if (newBook != null)
                    SelectedItems.Add(new ApiBook(newBook));
            }

            //Create select cells with the already selected books = true
            Items = new MvxObservableCollection<CellBookSelect>();
            books.Docs.ForEach(x => Items.Add(
                new CellBookSelect(x, SelectedItems.Contains(x))));

            _currentPage = books.Page;
            _hasNextPage = books.HasNextPage;
            IsLoading = false;
        }


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