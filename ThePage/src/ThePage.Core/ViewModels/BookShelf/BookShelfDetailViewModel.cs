using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookShelfDetailViewModel : BaseViewModel<Bookshelf, bool>
    {
        readonly IMvxNavigationService _navigation;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        readonly IBookShelfService _bookShelfService;

        Bookshelf _bookShelf;

        #region Properties

        public override string LblTitle => _bookShelf != null ? _bookShelf.Name : "Bookshelf detail";

        public BookshelfDetail BookShelfDetail { get; internal set; }

        public MvxObservableCollection<ICell> Items { get; set; } = new MvxObservableCollection<ICell>();

        public bool IsEditing { get; set; }

        public bool IsValid { get; internal set; }

        public string LblBtnUpdate => "Update bookshelf";

        public string LblBtnDelete => "Delete bookshelf";

        readonly MvxInteraction _updateToolbarInteraction = new MvxInteraction();
        public IMvxInteraction UpdateToolbarInteraction => _updateToolbarInteraction;

        #endregion

        #region Commands

        IMvxCommand _editBookShelfCommand;
        public IMvxCommand EditBookShelfCommand => _editBookShelfCommand ??= new MvxCommand(ToggleEditValue);

        IMvxAsyncCommand _commandUpdateBookShelf;
        public IMvxAsyncCommand CommandUpdateBookShelf => _commandUpdateBookShelf ??= new MvxAsyncCommand(UpdateBookShelf);

        IMvxAsyncCommand _commandDeleteBookShelf;
        public IMvxAsyncCommand CommandDeleteBookShelf => _commandDeleteBookShelf ??= new MvxAsyncCommand(DeleteBookShelf);

        #endregion

        #region Constructor

        public BookShelfDetailViewModel(IMvxNavigationService navigation,
                                        IUserInteraction userInteraction,
                                        IDevice device,
                                        IBookShelfService bookShelfService)
        {
            _navigation = navigation;
            _userInteraction = userInteraction;
            _device = device;

            _bookShelfService = bookShelfService;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(Bookshelf parameter)
        {
            _bookShelf = parameter;
        }

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookShelfDetailViewModel)}");
            await base.Initialize();

            await Refresh();
        }

        #endregion

        #region Private

        async Task Refresh()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            BookShelfDetail = await _bookShelfService.FetchBookShelf(_bookShelf.Id);

            Items = new MvxObservableCollection<ICell>
            {
                new BaseCellTitle("Name"),
                new CellBookShelfTextView(EBookShelfInputType.Name, BookShelfDetail.Name,UpdateValidation),
                new BaseCellTitle("List of books:")
            };

            BookShelfDetail.Books.ForEach(x => Items.Add(new CellBookShelfBookItem(x, RemoveBook, (obj) => GoToBookDetail(obj).Forget())));

            IsLoading = false;
            UpdateValidation();
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.OfType<BaseCellInput>().ToList();
            var HasInvalidCells = lstInput.Any(x => !x.IsValid);

            IsValid = !HasInvalidCells;
        }

        async Task AddBooks()
        {
            _device.HideKeyboard();

            var selectedBooks = Items.Where(b => b is CellBookShelfBookItem)
                .OfType<CellBookShelfBookItem>()
                .Select(i => i.Book).ToList();
            var books = await _navigation.Navigate<BookSelectViewModel, List<Book>, List<Book>>(selectedBooks);

            if (books != null)
            {
                //Remove all old books:
                Items.RemoveItems(Items.OfType<CellBookShelfBookItem>().ToList());

                var bookItems = new List<CellBookShelfBookItem>();
                books.ForEach(x => bookItems.Add(new CellBookShelfBookItem(x, RemoveBook, (obj) => GoToBookDetail(obj).Forget(), isEdit: true)));

                Items.InsertRange(Items.Count, bookItems);
            }
        }

        void RemoveBook(ICell obj)
        {
            Items.Remove(obj);
        }

        async Task GoToBookDetail(ICell obj)
        {
            if (obj is CellBookShelfBookItem bookshelfItem)
            {
                var result = await _navigation.Navigate<BookDetailViewModel, BookDetailParameter, bool>(new BookDetailParameter(bookshelfItem.Book));
                if (result)
                    await Refresh();
            }
        }

        void ToggleEditValue()
        {
            _device.HideKeyboard();
            IsEditing = !IsEditing;

            //Set all input or info fields visible
            foreach (var item in Items.OfType<BaseCellInput>())
                item.IsEdit = IsEditing;

            if (IsEditing)
            {
                var index = Items.FindIndex(x => x is CellBookShelfBookItem);
                if (index == -1)
                    index = Items.Count;

                Items.Insert(index, new BaseCellClickableText("Add a book", AddBooks));
                UpdateValidation();
            }
            else
            {
                Items.Remove(Items.OfType<BaseCellClickableText>().First());
            }
        }

        async Task UpdateBookShelf()
        {
            if (IsLoading)
                return;

            _device.HideKeyboard();
            IsLoading = true;

            var request = UpdateBookShelfCellData();

            if (request != null)
                await _bookShelfService.UpdateBookShelf(request);

            ToggleEditValue();
            IsLoading = false;
        }

        async Task DeleteBookShelf()
        {
            if (IsLoading)
                return;

            var answer = await _userInteraction.ConfirmAsync("Remove bookshelf?", "Confirm", "DELETE");
            if (answer)
            {
                IsLoading = true;

                var result = await _bookShelfService.DeleteBookShelf(BookShelfDetail.Id);
                if (result)
                    await _navigation.Close(this, true);
                else
                    IsLoading = false;
            }
        }

        ApiBookShelfRequest UpdateBookShelfCellData()
        {
            var (request, books) = BookShelfBusinessLogic.CreateApiBookShelfRequestFromInput(Items, _bookShelf.Id, BookShelfDetail);

            if (request == null)
                return null;

            if (request.Name != null)
            {
                BookShelfDetail.Name = request.Name;
                _bookShelf.Name = request.Name;

                _updateToolbarInteraction.Raise();
            }

            if (request.Books != null)
            {
                BookShelfDetail.Books = books.ToList();
                _bookShelf.Books = request.Books;
            }

            return request;
        }

        #endregion
    }
}