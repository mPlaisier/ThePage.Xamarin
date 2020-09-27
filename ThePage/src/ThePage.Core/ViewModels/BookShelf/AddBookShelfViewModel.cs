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
    public class AddBookShelfViewModel : BaseViewModelResult<bool>, INotifyPropertyChanged
    {
        readonly IMvxNavigationService _navigation;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;
        readonly IDevice _device;

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

        public AddBookShelfViewModel(IMvxNavigationService navigation, IThePageService thePageService, IUserInteraction userInteraction, IDevice device)
        {
            _navigation = navigation;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
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
            var books = await _navigation.Navigate<BookSelectViewModel, List<ApiBook>, List<ApiBook>>(selectedBooks);

            if (books != null)
            {
                //Remove all old books:
                Items.RemoveItems(Items.OfType<CellBookShelfBookItem>().ToList());

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

            var request = BookShelfBusinessLogic.CreateApiBookShelfRequestFromInput(Items);
            var result = await _thePageService.AddBookShelf(request);

            if (result)
            {
                _userInteraction.ToastMessage("Bookshelf added");
                await _navigation.Close(this, true);
            }
            else
            {
                _userInteraction.Alert("Failure adding bookshelf");
                IsLoading = false;
            }
        }

        void UpdateValidation()
        {
            if (IsLoading)
                return;

            var lstInput = Items.OfType<BaseCellInput>().ToList();
            var isValid = lstInput.Where(x => x.IsValid == false).Count() == 0;

            IsValid = isValid;
        }

        #endregion
    }
}