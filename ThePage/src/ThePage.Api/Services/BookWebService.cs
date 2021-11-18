using System.Threading.Tasks;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class BookWebService : IBookWebService
    {
        readonly ITokenWebService _webService;
        readonly ILocalDatabaseService _localDatabase;

        #region Constructor

        public BookWebService(ITokenWebService webService, ILocalDatabaseService localDatabase)
        {
            _webService = webService;
            _localDatabase = localDatabase;
        }

        #endregion

        #region Public

        public async Task<ApiBookResponse> GetList(int? page = null)
        {
            var result = _localDatabase.GetData<ApiBookResponse>(EApiDataType.BookList, null, page);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IBookApi>();
                result = await api.GetBooks(new ApiPageRequest(page));
                _localDatabase.StoreData(result, EApiDataType.BookList, page: page);
            }

            return result;
        }

        public async Task<ApiBookDetailResponse> GetDetail(string id)
        {
            var result = _localDatabase.GetData<ApiBookDetailResponse>(EApiDataType.BookDetail, id);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IBookApi>();
                result = await api.GetBook(id);
                _localDatabase.StoreData(result, EApiDataType.BookDetail, id);
            }

            return result;
        }

        public async Task<ApiBookResponse> SearchTitle(string search, int? page = null)
        {
            var api = await _webService.GetApi<IBookApi>();
            return await api.SearchTitle(new ApiSearchRequest(page, search));
        }

        public async Task<ApiBookDetailResponse> Add(ApiBookDetailRequest book)
        {
            _localDatabase.Clear(EApiDataType.BookList);

            var api = await _webService.GetApi<IBookApi>();
            return await api.AddBook(book);
        }

        public async Task<ApiBookDetailResponse> Update(string id, ApiBookDetailRequest book)
        {
            _localDatabase.Clear(EApiDataType.BookList);
            _localDatabase.Clear(EApiDataType.BookDetail, id);

            var api = await _webService.GetApi<IBookApi>();
            return await api.UpdateBook(book, id);
        }

        public async Task Delete(string id)
        {
            _localDatabase.Clear(EApiDataType.BookList);
            _localDatabase.Clear(EApiDataType.BookDetail, id);

            var api = await _webService.GetApi<IBookApi>();
            await api.DeleteBook(id);
        }

        #endregion
    }
}
