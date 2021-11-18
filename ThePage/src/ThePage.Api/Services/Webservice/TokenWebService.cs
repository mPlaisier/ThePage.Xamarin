using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class TokenWebService : BaseWebService, ITokenWebService
    {
        readonly ITokenService _tokenService;
        readonly IAuthenticationWebService _authenticationWebService;

        #region Constructor

        public TokenWebService(ITokenService tokenService, IAuthenticationWebService authenticationWebService)
        {
            _tokenService = tokenService;
            _authenticationWebService = authenticationWebService;
        }

        #endregion

        #region Public

        /// <summary>
        /// Create or fetch the instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isWithToken"></param>
        /// <returns></returns>
        public override async Task<T> GetApi<T>()
        {
            var api = GetApiInstance<T>();

            var shouldRefresh = _tokenService.ShouldRefreshToken();
            if (shouldRefresh || api == null)
            {
                var client = await GetHttpClient(shouldRefresh);
                api = RestService.For<T>(client);
                SetApiInstance(api);
            }

            return api;
        }

        #endregion

        #region Private

        async Task<HttpClient> GetHttpClient(bool shouldRefresh)
        {
            if (_client == null || shouldRefresh)
            {
                var token = await _authenticationWebService.GetAccessToken();
                _client = HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token);
            }
            return _client;
        }

        #endregion
    }
}
