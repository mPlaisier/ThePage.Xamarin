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
    public class BookSelectViewModel : BaseListViewModel<List<Book>, List<Book>>,
                                       IBaseSelectMultipleItemsViewModel<CellBookSelect, Book>
    {
        readonly IMvxNavigationService _navigation;
        readonly IBookService _bookService;

        #region Properties

        public override string LblTitle => "Select Book";

        public MvxObservableCollection<CellBookSelect> Items { get; set; } = new MvxObservableCollection<CellBookSelect>();

        public List<Book> SelectedItems { get; internal set; }

        #endregion

        #region Commandds

        IMvxCommand<CellBookSelect> _commandSelectItem;
        public IMvxCommand<CellBookSelect> CommandSelectItem => _commandSelectItem ??= new MvxCommand<CellBookSelect>(HandleBookClick);

        IMvxAsyncCommand _commandAddItem;
        public IMvxAsyncCommand CommandAddItem => _commandAddItem ??= new MvxAsyncCommand(async () =>
        {
            var result = await _navigation.Navigate<AddBookViewModel, string>();
            if (result != null)
            {
                //Add new created book to Selected list
                await AddNewCreatedBookToList(result);
                await Refresh();
            }

        });

        IMvxCommand _commandConfirm;
        public IMvxCommand CommandConfirm => _commandConfirm ??= new MvxCommand(HandleConfirm);

        #endregion

        #region Constructor

        public BookSelectViewModel(IMvxNavigationService navigationService,
                                   IBookService bookService)
        {
            _navigation = navigationService;
            _bookService = bookService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(List<Book> parameter)
        {
            SelectedItems = parameter ?? new List<Book>();
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
                var books = await _bookService.LoadNextBooks();
                var cells = books.Select(x => new CellBookSelect(x, SelectedItems.Contains(x)));
                Items.AddRange(cells);
            }
        }

        public override async Task Search(string search)
        {
            if (IsLoading)
                return;

            var currentSearch = _bookService.SearchText;
            if (currentSearch != null && currentSearch.Equals(search))
                return;

            IsLoading = true;

            var books = await _bookService.Search(search);
            var cells = books.Select(x => new CellBookSelect(x, SelectedItems.Contains(x)));
            Items = new MvxObservableCollection<CellBookSelect>(cells);

            IsLoading = false;
        }

        public override async void StopSearch()
        {
            if (_bookService.IsSearching)
                await Refresh();
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            IsLoading = true;

            var books = await _bookService.FetchBooks();
            var cells = books.Select(x => new CellBookSelect(x, SelectedItems.Contains(x)));

            if (cells.IsNotNull())
                Items = new MvxObservableCollection<CellBookSelect>(cells);

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

        async Task AddNewCreatedBookToList(string id)
        {
            if (id != null)
            {
                IsLoading = true;

                var bookDetail = await _bookService.FetchBook(id);
                if (bookDetail != null)
                    SelectedItems.Add(BookBusinessLogic.MapBook(bookDetail));
            }
        }

        #endregion
    }
}