using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class TokenlessWebService : BaseWebService, ITokenlessWebService
    {
        protected readonly Dictionary<Type, HttpClient> _dictClient = new Dictionary<Type, HttpClient>();

        #region Public

        /// <summary>
        /// Create or fetch the instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isWithToken"></param>
        /// <returns></returns>
        public override Task<T> GetApi<T>()
        {
            var api = GetApiInstance<T>();

            if (api == null)
            {
                var client = GetHttpClient(typeof(T));
                api = RestService.For<T>(client);
                SetApiInstance(api);
            }

            return Task.FromResult(api);
        }

        #endregion

        #region Private

        HttpClient GetHttpClient(Type type)
        {
            if (!_dictClient.ContainsKey(type))
            {
                var client = HttpUtils.GetHttpClient(GetUrl(type));
                _dictClient.Add(type, client);
            }

            return _dictClient[type];
        }

        string GetUrl(Type type)
        {
            if (type == typeof(IGoogleBooksApi))
                return Constants.Google_Books_Url;
            else if (type == typeof(IOpenLibraryApi))
                return Constants.OpenLibrary_Api_Url;
            else
                return Constants.ThePage_Api_Url;
        }

        #endregion
    }
}
