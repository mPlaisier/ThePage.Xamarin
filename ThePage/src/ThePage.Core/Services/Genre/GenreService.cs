using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public class GenreService : IGenreService
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

        public GenreService(IDevice device, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _device = device;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region Public

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            IsSearching = false;
            SearchText = null;

            var response = await _thePageService.GetAllGenres();
            _currentPage = response.Page;
            _hasNextPage = response.HasNextPage;

            var genres = GenreBusinessLogic.MapGenres(response.Docs);
            return genres;
        }

        public async Task<Genre> GetGenre(string id)
        {
            var response = await _thePageService.GetGenre(id);
            return GenreBusinessLogic.MapGenre(response);
        }

        public async Task<IEnumerable<Genre>> LoadNextGenres()
        {
            if (_hasNextPage && !_isLoadingNextPage)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var response = IsSearching
                    ? await _thePageService.SearchGenres(SearchText, _currentPage + 1)
                    : await _thePageService.GetNextGenres(_currentPage + 1);

                var genres = GenreBusinessLogic.MapGenres(response.Docs);

                _currentPage = response.Page;
                _hasNextPage = response.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
                return genres;
            }
            return Enumerable.Empty<Genre>();
        }

        public async Task<IEnumerable<Genre>> Search(string search)
        {
            _device.HideKeyboard();

            if (SearchText != null && SearchText.Equals(search))
                return Enumerable.Empty<Genre>();

            SearchText = search;
            IsSearching = true;

            var response = await _thePageService.SearchGenres(search);

            var genres = GenreBusinessLogic.MapGenres(response.Docs);

            _currentPage = response.Page;
            _hasNextPage = response.HasNextPage;

            return genres;
        }

        public async Task<Genre> AddGenre(string name)
        {
            _device.HideKeyboard();

            var result = await _thePageService.AddGenre(new ApiGenreRequest(name.Trim()));
            if (result != null)
            {
                _userInteraction.ToastMessage("Genre added", EToastType.Success);
                return GenreBusinessLogic.MapGenre(result);
            }
            else
            {
                _userInteraction.Alert("Failure adding genre");
            }

            return null;
        }

        public async Task<bool> DeleteGenre(string id)
        {
            var result = await _thePageService.DeleteGenre(id);

            if (result)
            {
                _userInteraction.ToastMessage("Genre removed", EToastType.Success);
                return true;
            }
            else
            {
                _userInteraction.Alert("Failure removing genre");
                return false;
            }
        }

        #endregion
    }
}