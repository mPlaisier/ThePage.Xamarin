using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class BookShelfDetailViewModel : BaseViewModel<ApiBookShelf, bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

        ApiBookShelf _bookShelf;

        #region Properties

        public override string LblTitle => _bookShelf != null ? _bookShelf.Name : "Bookshelf detail";

        public ApiBookShelfDetailResponse BookShelfDetail { get; internal set; }

        public MvxObservableCollection<ICell> Items { get; set; } = new MvxObservableCollection<ICell>();

        public bool IsEditing { get; set; }

        public bool IsValid { get; internal set; }

        public string LblBtnUpdate => "Update bookshelf";

        public string LblBtnDelete => "Delete bookshelf";

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

        public BookShelfDetailViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override void Prepare(ApiBookShelf parameter)
        {
            _bookShelf = parameter;
        }

        public override async Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(BookShelfDetailViewModel)}");
            await base.Initialize();

            FetchData().Forget();
        }

        #endregion

        #region Private

        async Task FetchData()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            BookShelfDetail = await _thePageService.GetBookShelf(_bookShelf.Id);

            Items = new MvxObservableCollection<ICell>
            {
                new BaseCellTitle("Name"),
                new CellBookShelfTextView(EBookShelfInputType.Name, BookShelfDetail.Name,UpdateValidation),
                new BaseCellTitle("List of books:")
            };

            BookShelfDetail.Books.ForEach(x => Items.Add(new CellBookShelfBookItem(x, RemoveBook)));

            IsLoading = false;
            UpdateValidation();
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.OfType<BaseCellInput>().ToList();
            var isValid = lstInput.Where(x => x.IsValid == false).Count() == 0;

            IsValid = isValid;
        }

        async Task AddBooks()
        {
            _device.HideKeyboard();

            var selectedBooks = Items.Where(b => b is CellBookShelfBookItem)
                .OfType<CellBookShelfBookItem>()
                .Select(i => i.Book).ToList();
            var books = await _navigation.Navigate<BookSelectViewModel, List<ApiBook>, List<ApiBook>>(selectedBooks);

            if (books != null)
            {
                //Remove all old books:
                Items.RemoveItems(Items.OfType<CellBookShelfBookItem>().ToList());

                var bookItems = new List<CellBookShelfBookItem>();
                books.ForEach(x => bookItems.Add(new CellBookShelfBookItem(x, RemoveBook)));

                Items.InsertRange(3, bookItems);
            }
        }

        void RemoveBook(ICell obj)
        {
            Items.Remove(obj);
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
                Items.Insert(Items.Count, new BaseCellClickableText("Add a book", AddBooks));
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
            {
                var result = await _thePageService.UpdateBookShelf(BookShelfDetail.Id, request);

                if (result != null)
                    _userInteraction.ToastMessage("Bookshelf updated", EToastType.Success);
                else
                    _userInteraction.Alert("Failure updating bookshelf");
            }

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

                var result = await _thePageService.DeleteBookShelf(BookShelfDetail);

                if (result)
                {
                    _userInteraction.ToastMessage("Bookshelf removed", EToastType.Success);
                    await _navigation.Close(this, true);
                }
                else
                {
                    _userInteraction.Alert("Failure removing bookshelf");
                    IsLoading = false;
                }
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
