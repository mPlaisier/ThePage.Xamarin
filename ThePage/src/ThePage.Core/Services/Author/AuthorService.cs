using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Navigation;
using ThePage.Api;

namespace ThePage.Core
{
    public class AuthorService : IAuthorService
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

        public AuthorService(IDevice device, IMvxNavigationService navigationService, IThePageService thePageService, IUserInteraction userInteraction)
        {
            _device = device;
            _navigationService = navigationService;
            _thePageService = thePageService;
            _userInteraction = userInteraction;
        }

        #endregion

        #region Public

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            IsSearching = false;
            SearchText = null;

            var apiAuthorResponse = await _thePageService.GetAllAuthors();
            _currentPage = apiAuthorResponse.Page;
            _hasNextPage = apiAuthorResponse.HasNextPage;

            var authors = AuthorBusinessLogic.ConvertApiAuthorsToAuthors(apiAuthorResponse.Docs);

            return authors;
        }

        public async Task<IEnumerable<Author>> LoadNextAuthors()
        {
            if (_hasNextPage && !_isLoadingNextPage)
            {
                _isLoadingNextPage = true;
                _userInteraction.ToastMessage("Loading data", EToastType.Info);

                var apiAuthorResponse = IsSearching
                    ? await _thePageService.SearchAuthors(SearchText, _currentPage + 1)
                    : await _thePageService.GetNextAuthors(_currentPage + 1);

                var authors = AuthorBusinessLogic.ConvertApiAuthorsToAuthors(apiAuthorResponse.Docs);

                _currentPage = apiAuthorResponse.Page;
                _hasNextPage = apiAuthorResponse.HasNextPage;
                _isLoadingNextPage = false;

                _userInteraction.ToastMessage("Data loaded", EToastType.Success);
                return authors;
            }
            return Enumerable.Empty<Author>();
        }

        public async Task<IEnumerable<Author>> Search(string search)
        {
            _device.HideKeyboard();

            if (SearchText != null && SearchText.Equals(search))
                return Enumerable.Empty<Author>();

            SearchText = search;
            IsSearching = true;

            var apiAuthorResponse = await _thePageService.SearchAuthors(search);

            var authors = AuthorBusinessLogic.ConvertApiAuthorsToAuthors(apiAuthorResponse.Docs);

            _currentPage = apiAuthorResponse.Page;
            _hasNextPage = apiAuthorResponse.HasNextPage;

            return authors;
        }

        public async Task<Author> AddAuthor(string name)
        {
            var result = await _thePageService.AddAuthor(new ApiAuthorRequest(name.Trim()));
            if (result != null)
            {
                _userInteraction.ToastMessage("Author added");
                return AuthorBusinessLogic.MapAuthor(result);
            }
            else
            {
                _userInteraction.Alert("Failure adding author");
            }

            return null;
        }

        public async Task<Author> UpdateAuthor(Author author)
        {
            var request = new ApiAuthorRequest(author.Name, author.Olkey);
            var result = await _thePageService.UpdateAuthor(author.Id, request);

            return AuthorBusinessLogic.MapAuthor(result);
        }

        #endregion
    }
}