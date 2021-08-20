using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    [ThePageLazySingletonService]
    public class BookShelfService : IBookShelfService
    {
        readonly IDevice _device;
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

        public BookShelfService(IDevice device, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _device = device;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region Public

        public async Task<IEnumerable<Bookshelf>> FetchBookshelves()
        {
            IsSearching = false;
            SearchText = null;

            var response = await _thePageService.GetAllBookShelves();
            _currentPage = response.Page;
            _hasNextPage = response.HasNextPage;

            var bookshelves = BookShelfBusinessLogic.MapBookshelves(response.Docs);

            return bookshelves;
        }

        public async Task<IEnumerable<Bookshelf>> LoadNextBookshelves()
        {
            if (_hasNextPage && !_isLoadingNextPage)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var response = IsSearching
                    ? await _thePageService.SearchBookshelves(SearchText, _currentPage + 1)
                    : await _thePageService.GetNextBookshelves(_currentPage + 1);

                var bookshelves = BookShelfBusinessLogic.MapBookshelves(response.Docs);

                _currentPage = response.Page;
                _hasNextPage = response.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
                return bookshelves;
            }
            return Enumerable.Empty<Bookshelf>();
        }

        public async Task<IEnumerable<Bookshelf>> Search(string search)
        {
            _device.HideKeyboard();

            if (SearchText != null && SearchText.Equals(search))
                return Enumerable.Empty<Bookshelf>();

            SearchText = search;
            IsSearching = true;

            var response = await _thePageService.SearchBookshelves(search);

            var bookshelves = BookShelfBusinessLogic.MapBookshelves(response.Docs);

            _currentPage = response.Page;
            _hasNextPage = response.HasNextPage;

            return bookshelves;
        }

        public async Task<bool> AddBookshelf(MvxObservableCollection<ICell> cells)
        {
            var request = BookShelfBusinessLogic.CreateApiBookShelfRequestFromInput(cells).request;
            var result = await _thePageService.AddBookShelf(request);

            if (result)
            {
                _userInteraction.ToastMessage("Bookshelf added");
                return true;
            }
            else
            {
                _userInteraction.Alert("Failure adding bookshelf");
            }

            return false;
        }

        public async Task<BookshelfDetail> FetchBookShelf(string id)
        {
            var response = await _thePageService.GetBookShelf(id);

            var bookshelf = BookShelfBusinessLogic.MapBookshelfDetail(response);

            return bookshelf;
        }

        public async Task<bool> UpdateBookShelf(ApiBookShelfRequest request)
        {
            var result = await _thePageService.UpdateBookShelf(request.Id, request);

            if (result != null)
            {
                _userInteraction.ToastMessage("Bookshelf updated", EToastType.Success);
                return true;
            }
            else
            {
                _userInteraction.Alert("Failure updating bookshelf");
                return false;
            }
        }

        public async Task<bool> DeleteBookShelf(string id)
        {
            var result = await _thePageService.DeleteBookShelf(id);
            if (result)
            {
                _userInteraction.ToastMessage("Bookshelf removed", EToastType.Success);
                return true;
            }
            else
            {
                _userInteraction.Alert("Failure removing bookshelf");
                return false;
            }
        }

        #endregion
    }
}