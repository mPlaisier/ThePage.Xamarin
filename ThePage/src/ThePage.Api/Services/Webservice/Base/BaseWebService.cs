using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThePage.Api
{
    public abstract class BaseWebService : IBaseWebService
    {
        protected readonly Dictionary<Type, IApi> _dictApi = new Dictionary<Type, IApi>();

        protected HttpClient _client;

        #region Public

        public abstract Task<T> GetApi<T>() where T : IApi;

        #endregion

        #region Protected

        protected T GetApiInstance<T>() where T : IApi
        {
            var type = typeof(T);

            if (_dictApi.ContainsKey(type))
                return (T)_dictApi[type];
            return default;
        }

        protected void SetApiInstance<T>(T api) where T : IApi
        {
            var type = typeof(T);

            if (_dictApi.ContainsKey(type))
                _dictApi[type] = api;
            else
                _dictApi.Add(type, api);
        }

        #endregion
    }
}
