using System.Threading.Tasks;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class AuthorWebService : IAuthorWebService
    {
        readonly ITokenWebService _webService;
        readonly ILocalDatabaseService _localDatabase;

        #region Constructor

        public AuthorWebService(ITokenWebService webService, ILocalDatabaseService localDatabase)
        {
            _webService = webService;
            _localDatabase = localDatabase;
        }

        #endregion

        #region Public

        public async Task<ApiAuthorResponse> GetList(int? page = null)
        {
            var result = _localDatabase.GetData<ApiAuthorResponse>(EApiDataType.AuthorList, null, page);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IAuthorApi>();
                result = await api.GetAuthors(new ApiPageRequest(page));
                _localDatabase.StoreData(result, EApiDataType.AuthorList, page: page);
            }

            return result;
        }

        public async Task<ApiAuthor> GetDetail(string id)
        {
            var result = _localDatabase.GetData<ApiAuthor>(EApiDataType.AuthorDetail, id);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IAuthorApi>();
                result = await api.GetAuthor(id);
                _localDatabase.StoreData(result, EApiDataType.AuthorDetail, id);
            }

            return result;
        }

        public async Task<ApiAuthorResponse> Search(string search, int? page = null)
        {
            var api = await _webService.GetApi<IAuthorApi>();
            return await api.SearchAuthors(new ApiSearchRequest(page, search));
        }

        public async Task<ApiAuthor> Add(ApiAuthorRequest author)
        {
            _localDatabase.Clear(EApiDataType.AuthorList);

            var api = await _webService.GetApi<IAuthorApi>();
            return await api.AddAuthor(author);
        }

        public async Task<ApiAuthor> Update(string id, ApiAuthorRequest request)
        {
            _localDatabase.Clear(EApiDataType.AuthorList);
            _localDatabase.Clear(EApiDataType.AuthorDetail, id);

            var api = await _webService.GetApi<IAuthorApi>();
            return await api.UpdateAuthor(request, id);
        }

        public async Task Delete(string id)
        {
            _localDatabase.Clear(EApiDataType.AuthorList);
            _localDatabase.Clear(EApiDataType.AuthorDetail, id);

            var api = await _webService.GetApi<IAuthorApi>();
            await api.DeleteAuthor(id);
        }

        #endregion
    }
}
