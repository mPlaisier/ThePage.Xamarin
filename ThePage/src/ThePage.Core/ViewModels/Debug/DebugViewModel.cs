using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using ThePage.Api;
using ThePage.Core.ViewModels;

namespace ThePage.Core
{
    public class DebugViewModel : BaseViewModel
    {
        readonly IUserInteraction _userInteraction;
        readonly IThePageService _thePageService;

        #region Properties

        public override string Title => "Debug Menu";

        public MvxObservableCollection<ICellDebug> Items { get; internal set; }

        MvxInteraction<GetIsbnCode> _isbnInteraction = new MvxInteraction<GetIsbnCode>();
        public IMvxInteraction<GetIsbnCode> ISBNInteraction => _isbnInteraction;

        #endregion

        #region Commands

        MvxCommand<ICellDebug> _itemClickCommand;
        public MvxCommand<ICellDebug> ItemClickCommand => _itemClickCommand = _itemClickCommand ?? new MvxCommand<ICellDebug>(OnItemClick);

        #endregion

        #region Constructor

        public DebugViewModel(IUserInteraction userInteraction, IThePageService thePageService)
        {
            _userInteraction = userInteraction;
            _thePageService = thePageService;
        }

        #endregion 

        #region LifeCycle

        public override Task Initialize()
        {
            Analytics.TrackEvent($"Initialize {nameof(DebugViewModel)}");

            CreateMenuItems();

            return base.Initialize();
        }

        #endregion

        #region Private

        void CreateMenuItems()
        {
            Items = new MvxObservableCollection<ICellDebug>
            {
                new CellDebugHeader("Alerts",EDebugType.Alert, false),
                new CellDebugHeader("Toast",EDebugType.Toast)
            };

            Items.AddRange(GetToastDebugItems());

            Items.Add(new CellDebugHeader("Data", EDebugType.Data));
            Items.AddRange(GetDataDebugItems());

            Items.Add(new CellDebugHeader("BarCode", EDebugType.BarcodeScanner));
            Items.AddRange(GetBarcodeDebugItems());

        }

        void OnItemClick(ICellDebug obj)
        {
            if (obj is CellDebugHeader cellHeader)
            {
                HandleHeaderClick(cellHeader);
            }
            else if (obj is CellDebugItem debug)
            {
                switch (debug.ItemType)
                {
                    case EDebugItemType.ConfirmOk:
                        _userInteraction.Confirm("Message", OkAction, "Custom title");
                        break;
                    case EDebugItemType.ConfirmAnswer:
                        _userInteraction.Confirm("Custom message", OkActionAnswer, "Custom title");
                        break;
                    case EDebugItemType.Alert:
                        _userInteraction.Alert("Custom message alert", AlertAction, "Custom Title");
                        break;
                    case EDebugItemType.InputOk:
                        _userInteraction.Input("Message", InputOK, "placeholder", "Custom title", "OK", "Cancel", "initial Text");
                        break;
                    case EDebugItemType.ConfirmThreeButtons:
                        _userInteraction.ConfirmThreeButtons("Custom message", ThreeButtonAnwer, "Custom Title");
                        break;

                    case EDebugItemType.Toast:
                        _userInteraction.ToastMessage("Custom message");
                        break;

                    case EDebugItemType.BookNotFound:
                        BookNotFoundCall().Forget();
                        break;
                    case EDebugItemType.CreateData:
                        CreateTestData().Forget();
                        break;
                    case EDebugItemType.RemoveAllData:
                        RemoveAllData().Forget();
                        break;

                    case EDebugItemType.BarcodeScanner:
                        StartBarcodeScanner();
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Alerts

        void OkAction()
        {
            System.Diagnostics.Debug.WriteLine("OK Action: Pressed OK", "DEBUG ALERTS");
        }

        void OkActionAnswer(bool answer)
        {
            System.Diagnostics.Debug.WriteLine($"OkActionAnswer Pressed {answer}", "DEBUG ALERTS");
        }

        void AlertAction()
        {
            System.Diagnostics.Debug.WriteLine("AlertAction Pressed OK", "DEBUG ALERTS");
        }

        private void InputOK(string obj)
        {
            System.Diagnostics.Debug.WriteLine($"InputOK Pressed {obj}", "DEBUG ALERTS");
        }

        private void ThreeButtonAnwer(ConfirmThreeButtonsResponse obj)
        {
            System.Diagnostics.Debug.WriteLine($"ThreeButtonAnwer Pressed {obj}", "DEBUG ALERTS");
        }

        #endregion

        #region Data

        async Task BookNotFoundCall()
        {
            await _thePageService.GetBook("5e4eca767f80d86dd7865ee8");
        }

        async Task RemoveAllData(bool shouldConfirm = true)
        {
            var confirm = true;
            if (shouldConfirm)
                confirm = await _userInteraction.ConfirmAsync("Remove all test data?");

            if (confirm)
            {
                IsLoading = true;

                var authors = await _thePageService.GetAllAuthors();
                var genres = await _thePageService.GetAllGenres();
                var books = await _thePageService.GetAllBooks();

                foreach (var author in authors.Docs)
                {
                    await _thePageService.DeleteAuthor(author);
                }

                foreach (var genre in genres.Docs)
                {
                    await _thePageService.DeleteGenre(genre);
                }

                //TODO
                //foreach (var book in books.Docs)
                //{
                //    await _thePageService.DeleteBook(book);
                //}

                IsLoading = false;
            }
        }

        async Task CreateTestData()
        {
            var confirm = await _userInteraction.ConfirmAsync("This will remove all current data and create new data?");
            if (confirm)
            {
                IsLoading = true;

                //Remove all current data
                await RemoveAllData(false);

                //Create Genres and Authors
                await CreateGenres();
                await CreateAuthors();

                //Fetch Data
                var genres = await _thePageService.GetAllGenres();
                var authors = await _thePageService.GetAllAuthors();

                //Create books
                await CreateBooks(genres.Docs, authors.Docs);

                IsLoading = false;
            }

        }

        async Task CreateGenres()
        {
            int amountOfGenres = 10;

            for (int i = 0; i < amountOfGenres; i++)
            {
                await _thePageService.AddGenre(new ApiGenreRequest($"Genre {i + 1}"));
            }
        }

        async Task CreateAuthors()
        {
            int amountOfAuthors = 10;
            for (int i = 0; i < amountOfAuthors; i++)
            {
                await _thePageService.AddAuthor(new ApiAuthorRequest($"Author {i + 1}"));
            }
        }

        async Task CreateBooks(List<ApiGenre> genres, List<ApiAuthor> authors)
        {
            var random = new Random();
            int amountOfBooks = 25;
            int minGenres = 0;
            int maxGenres = 4;

            for (int i = 0; i < amountOfBooks; i++)
            {
                var amountGenres = random.Next(minGenres, maxGenres);
                var selectedgenres = new List<ApiGenre>();
                while (selectedgenres.Count < amountGenres)
                {
                    var genre = genres[random.Next(0, genres.Count() - 1)];
                    if (!selectedgenres.Contains(genre))
                        selectedgenres.Add(genre);
                }

                var author = authors[random.Next(0, authors.Count() - 1)];

                var book = new ApiBookDetailRequest($"Book {i + 1}",
                                                    author.Id,
                                                    selectedgenres.GetIdStrings().ToList(),
                                                    "",
                                                    false,
                                                    true,
                                                    random.Next(50, 500),
                                                    false,
                                                    string.Empty,
                                                    null);

                await _thePageService.AddBook(book);
            }
        }

        #endregion

        #region BarcodeScanner

        void StartBarcodeScanner()
        {
            var request = new GetIsbnCode
            {
                ISBNCallback = (isbn) =>
                {
                    if (isbn == null)
                        _userInteraction.Confirm("ISBN:" + isbn, OkAction, "Error");
                    else
                        _userInteraction.Confirm("ISBN:" + isbn, OkAction, "Success");
                }
            };

            _isbnInteraction.Raise(request);
        }

        public class GetIsbnCode
        {
            public Action<string> ISBNCallback { get; set; }
        }

        #endregion

        #region Private

        void HandleHeaderClick(CellDebugHeader cell)
        {
            //Close section
            if (cell.IsOpen)
            {
                var removeItems = Items.OfType<CellDebugItem>().Where(x => x.Type == cell.DebugType);
                Items.RemoveRange(removeItems);
            }
            else
            {
                IEnumerable<ICellDebug> newItems = null;

                switch (cell.DebugType)
                {
                    case EDebugType.Alert:
                        newItems = GetAlertDebugItems();
                        break;
                    case EDebugType.Toast:
                        newItems = GetToastDebugItems();
                        break;
                    case EDebugType.Data:
                        newItems = GetDataDebugItems();
                        break;
                    case EDebugType.BarcodeScanner:
                        newItems = GetBarcodeDebugItems();
                        break;
                    default:
                        break;
                }

                var index = Items.FindIndex(x => x is CellDebugHeader y && y.DebugType == cell.DebugType);
                Items.InsertRange(index + 1, newItems);
            }

            cell.IsOpen = !cell.IsOpen;
        }

        IEnumerable<ICellDebug> GetAlertDebugItems()
        {
            var list = new List<ICellDebug>(){
                new CellDebugItem("Confirm OK", EDebugType.Alert, EDebugItemType.ConfirmOk),
                new CellDebugItem("Confirm answer", EDebugType.Alert, EDebugItemType.ConfirmAnswer),
                //new CellDebugItem("Confirm async", EDebugType.Alert, EDebugItemType.ConfirmAsync),
                new CellDebugItem("Alert", EDebugType.Alert, EDebugItemType.Alert),
                //new CellDebugItem("Alert async", EDebugType.Alert, EDebugItemType.AlerAsync),
                new CellDebugItem("Input ok", EDebugType.Alert, EDebugItemType.InputOk),
                //new CellDebugItem("Input Answer", EDebugType.Alert, EDebugItemType.InputAnwser),
                //new CellDebugItem("Input Async", EDebugType.Alert, EDebugItemType.InputAsync),
                new CellDebugItem("Confirm three buttons", EDebugType.Alert, EDebugItemType.ConfirmThreeButtons),
               // new CellDebugItem("Confirm three buttons async", debugTypeAlert, EDebugItemType.ConfirmThreeButtonsAsync),
            };

            return list;
        }

        IEnumerable<ICellDebug> GetBarcodeDebugItems()
        {
            return new List<ICellDebug>
            {
               new CellDebugItem("Open Scanner", EDebugType.BarcodeScanner,EDebugItemType.BarcodeScanner)
            };
        }

        IEnumerable<ICellDebug> GetDataDebugItems()
        {
            return new List<ICellDebug>
            {
               new CellDebugItem("Book not found error", EDebugType.Data,EDebugItemType.BookNotFound),
               new CellDebugItem("Create test data", EDebugType.Data, EDebugItemType.CreateData),
               new CellDebugItem("Remove all data", EDebugType.Data, EDebugItemType.RemoveAllData)
            };
        }

        IEnumerable<ICellDebug> GetToastDebugItems()
        {
            return new List<ICellDebug>
            {
               new CellDebugItem("Toast message", EDebugType.Toast,EDebugItemType.Toast)
            };
        }

        #endregion
    }
}