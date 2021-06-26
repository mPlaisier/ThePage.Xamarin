using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Navigation;
using ThePage.Api;

namespace ThePage.Core
{
    public class BookService : IBookService
    {
        readonly IDevice _device;
        readonly IMvxNavigationService _navigationService;
        readonly IThePageService _thePageService;
        readonly IUserInteraction _userInteraction;

        int _currentPage;
        bool _hasNextPage;
        bool _isLoadingNextPage;

        #region Properties

        public string SearchText { get; private set; }

        public bool IsSearching { get; private set; }

        #endregion

        #region Constructor

        public BookService(IDevice device, IMvxNavigationService navigationService, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _device = device;
            _navigationService = navigationService;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region Public

        public Task<List<Book>> SelectBook(List<Book> books)
        {
            _device.HideKeyboard();

            return _navigationService.Navigate<BookSelectViewModel, List<Book>, List<Book>>(books);
        }

        public async Task<BookDetail> FetchBook(string id)
        {
            var apiBookResponse = await _thePageService.GetBook(id);
            return BookBusinessLogic.MapBookDetail(apiBookResponse);
        }

        public async Task<IEnumerable<Book>> FetchBooks()
        {
            IsSearching = false;
            SearchText = null;

            var apiBookResponse = await _thePageService.GetAllBooks();
            _currentPage = apiBookResponse.Page;
            _hasNextPage = apiBookResponse.HasNextPage;

            var books = BookBusinessLogic.MapBooks(apiBookResponse.Docs);
            return books;
        }

        public async Task<IEnumerable<Book>> LoadNextBooks()
        {
            if (_hasNextPage && !_isLoadingNextPage)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiBooksResponse = IsSearching
                    ? await _thePageService.SearchBooksTitle(SearchText, _currentPage + 1)
                    : await _thePageService.GetNextBooks(_currentPage + 1);

                var books = BookBusinessLogic.MapBooks(apiBooksResponse.Docs);

                _currentPage = apiBooksResponse.Page;
                _hasNextPage = apiBooksResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
                return books;
            }
            return Enumerable.Empty<Book>();
        }

        public async Task<IEnumerable<Book>> Search(string search)
        {
            _device.HideKeyboard();

            if (SearchText != null && SearchText.Equals(search))
                return Enumerable.Empty<Book>();

            SearchText = search;
            IsSearching = true;

            var apiBooksResponse = await _thePageService.SearchBooksTitle(search);

            var books = BookBusinessLogic.MapBooks(apiBooksResponse.Docs);

            _currentPage = apiBooksResponse.Page;
            _hasNextPage = apiBooksResponse.HasNextPage;

            return books;
        }

        public async Task<BookDetail> AddBook(IEnumerable<ICellBook> cells)
        {
            _device.HideKeyboard();

            var request = BookBusinessLogic.CreateBookDetailRequestFromInput(cells).request;
            var result = await _thePageService.AddBook(request);

            if (result != null)
            {
                _userInteraction.ToastMessage("Book added");
                return BookBusinessLogic.MapBookDetail(result);
            }
            else
            {
                _userInteraction.Alert("Failure adding book");
                return null;
            }
        }

        public async Task<bool> UpdateBook(ApiBookDetailRequest request)
        {
            var result = await _thePageService.UpdateBook(request.Id, request);

            if (result != null)
                _userInteraction.ToastMessage("Book updated", EToastType.Success);
            else
                _userInteraction.Alert("Failure updating book");

            return result.IsNotNull();
        }

        public async Task<bool> DeleteBook(string id)
        {
            var result = await _thePageService.DeleteBook(id);

            if (result)
            {
                _userInteraction.ToastMessage("Book removed");
                return true;
            }
            else
            {
                _userInteraction.Alert("Failure removing book");
                return false;
            }
        }

        #endregion
    }
}