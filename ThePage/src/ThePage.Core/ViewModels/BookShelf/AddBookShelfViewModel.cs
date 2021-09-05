using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class AddBookShelfViewModel : BaseViewModelResult<bool>
    {
        readonly IMvxNavigationService _navigation;
        readonly IDevice _device;
        readonly IBookShelfService _bookShelfService;

        #region Properties

        public override string LblTitle => "Add Bookshelf";

        public MvxObservableCollection<ICell> Items { get; set; }

        public bool IsValid { get; internal set; }

        public string LblBtn => "Add bookshelf";

        #endregion

        #region Commands

        IMvxAsyncCommand _commandAddBook;
        public IMvxAsyncCommand CommandAddBook => _commandAddBook ??= new MvxAsyncCommand(AddBooks);

        IMvxAsyncCommand _addBookShelfCommand;
        public IMvxAsyncCommand CommandAddBookShelf => _addBookShelfCommand ??= new MvxAsyncCommand(AddBookShelf);

        #endregion

        #region Constructor

        public AddBookShelfViewModel(IMvxNavigationService navigation, IBookShelfService bookShelfService, IDevice device)
        {
            _navigation = navigation;
            _bookShelfService = bookShelfService;
            _device = device;
        }

        #endregion

        #region LifeCycle

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(AddBookShelfViewModel)}");
            CreateItems();

            return base.Initialize();
        }

        #endregion

        #region Private

        void CreateItems()
        {
            Items = new MvxObservableCollection<ICell>
            {
                new BaseCellTitle("Name"),
                new CellBookShelfTextView(EBookShelfInputType.Name, UpdateValidation, isEdit: true),
                new BaseCellTitle("List of books:"),
                new BaseCellClickableText("Add a book", AddBooks)
            };
        }

        async Task AddBooks()
        {
            _device.HideKeyboard();

            var selectedBooks = Items.Where(g => g is CellBookShelfBookItem)
                .OfType<CellBookShelfBookItem>()
                .Select(i => i.Book).ToList();
            var books = await _navigation.Navigate<BookSelectViewModel, List<Book>, List<Book>>(selectedBooks);

            if (books != null)
            {
                //Remove all old books:
                Items.RemoveItems(Items.OfType<CellBookShelfBookItem>());

                var bookItems = new List<CellBookShelfBookItem>();
                books.ForEach(x => bookItems.Add(new CellBookShelfBookItem(x, RemoveBook, isEdit: true)));

                var index = Items.FindIndex(x => x is BaseCellClickableText);
                Items.InsertRange(index, bookItems);
            }
        }

        void RemoveBook(ICell obj)
        {
            Items.Remove(obj);
        }

        async Task AddBookShelf()
        {
            if (IsLoading)
                return;

            IsLoading = true;

            var result = await _bookShelfService.AddBookshelf(Items);
            if (result)
                await _navigation.Close(this, true);
            else
                IsLoading = false;
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.OfType<BaseCellInput>();
            IsValid = lstInput.All(x => x.IsValid);
        }

        #endregion
    }
}