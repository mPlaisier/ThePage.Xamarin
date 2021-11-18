using System.Threading.Tasks;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class BookShelfWebService : IBookShelfWebService
    {
        readonly ITokenWebService _webService;
        readonly ILocalDatabaseService _localDatabase;

        #region Constructor

        public BookShelfWebService(ITokenWebService webService, ILocalDatabaseService localDatabase)
        {
            _webService = webService;
            _localDatabase = localDatabase;
        }

        #endregion

        #region Public

        public async Task<ApiBookShelfResponse> GetList(int? page = null)
        {
            var result = _localDatabase.GetData<ApiBookShelfResponse>(EApiDataType.BookShelfList, null, page);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IBookShelfApi>();
                result = await api.GetBookShelves(new ApiPageRequest(page));
                _localDatabase.StoreData(result, EApiDataType.BookShelfList, page: page);
            }

            return result;
        }

        public async Task<ApiBookShelfDetailResponse> GetDetail(string id)
        {
            var result = _localDatabase.GetData<ApiBookShelfDetailResponse>(EApiDataType.BookShelfDetail, id);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IBookShelfApi>();
                result = await api.GetBookShelf(id);
                _localDatabase.StoreData(result, EApiDataType.BookShelfDetail, id);
            }

            return result;
        }

        public async Task<ApiBookShelfResponse> Search(string search, int? page = null)
        {
            var api = await _webService.GetApi<IBookShelfApi>();
            return await api.SearchBookshelves(new ApiSearchRequest(page, search));
        }

        public async Task<ApiBookShelf> Add(ApiBookShelfRequest bookshelf)
        {
            _localDatabase.Clear(EApiDataType.BookShelfList);

            var api = await _webService.GetApi<IBookShelfApi>();
            return await api.AddBookShelf(bookshelf);
        }

        public async Task<ApiBookShelfDetailResponse> Update(string id, ApiBookShelfRequest bookshelf)
        {
            _localDatabase.Clear(EApiDataType.BookShelfList);
            _localDatabase.Clear(EApiDataType.BookShelfDetail, id);

            var api = await _webService.GetApi<IBookShelfApi>();
            return await api.UpdateBookShelf(bookshelf, id);
        }

        public async Task Delete(string id)
        {
            _localDatabase.Clear(EApiDataType.BookShelfList);
            _localDatabase.Clear(EApiDataType.BookShelfDetail, id);

            var api = await _webService.GetApi<IBookShelfApi>();
            await api.DeleteBookShelf(id);
        }

        #endregion
    }
}
