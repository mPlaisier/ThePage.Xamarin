using System.Threading.Tasks;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class GenreWebService : IGenreWebService
    {
        readonly ITokenWebService _webService;
        readonly ILocalDatabaseService _localDatabase;

        #region Constructor

        public GenreWebService(ITokenWebService webService, ILocalDatabaseService localDatabase)
        {
            _webService = webService;
            _localDatabase = localDatabase;
        }

        #endregion

        #region Public

        public async Task<ApiGenreResponse> GetList(int? page = null)
        {
            var result = _localDatabase.GetData<ApiGenreResponse>(EApiDataType.GenreList, null, page);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IGenreApi>();
                result = await api.Get(new ApiPageRequest(page));
                _localDatabase.StoreData(result, EApiDataType.GenreList, page: page);
            }

            return result;
        }

        public async Task<ApiGenre> GetDetail(string id)
        {
            var result = _localDatabase.GetData<ApiGenre>(EApiDataType.GenreDetail, id);

            //No local data or expired
            if (result == null)
            {
                var api = await _webService.GetApi<IGenreApi>();
                result = await api.Get(id);
                _localDatabase.StoreData(result, EApiDataType.GenreDetail, id);
            }

            return result;
        }

        public async Task<ApiGenreResponse> Search(string search, int? page = null)
        {
            var api = await _webService.GetApi<IGenreApi>();
            return await api.SearchGenres(new ApiSearchRequest(page, search));
        }

        public async Task<ApiGenre> Add(ApiGenreRequest genre)
        {
            _localDatabase.Clear(EApiDataType.GenreList);

            var api = await _webService.GetApi<IGenreApi>();
            return await api.Add(genre);
        }

        public async Task<ApiGenre> Update(string id, ApiGenreRequest request)
        {
            _localDatabase.Clear(EApiDataType.GenreList);
            _localDatabase.Clear(EApiDataType.GenreDetail, id);

            var api = await _webService.GetApi<IGenreApi>();
            return await api.Update(request, id);
        }

        public async Task Delete(string id)
        {
            _localDatabase.Clear(EApiDataType.GenreList);
            _localDatabase.Clear(EApiDataType.GenreDetail, id);

            var api = await _webService.GetApi<IGenreApi>();
            await api.Delete(id);
        }

        #endregion
    }
}
