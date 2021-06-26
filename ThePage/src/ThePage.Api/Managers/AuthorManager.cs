using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;

namespace ThePage.Api
{
    public static class AuthorManager
    {
        const string AUTHORS_KEY = "GetAuthorsKey";
        const string AUTHORS_SINGLE_KEY = "GetAuthorKey";

        #region FETCH

        public static async Task<ApiAuthorResponse> Get(string token, int? page = null, bool forceRefresh = false)
        {
            var barrelkey = AUTHORS_KEY + (page ?? 1);

            ApiAuthorResponse result = null;
            if (!forceRefresh && Barrel.Current.Exists(barrelkey) && !Barrel.Current.IsExpired(barrelkey))
                result = Barrel.Current.Get<ApiAuthorResponse>(barrelkey);

            if (result == null)
            {
                var api = RestService.For<IAuthorApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));

                result = await api.GetAuthors(new ApiPageRequest(page));
                Barrel.Current.Add(barrelkey, result, TimeSpan.FromMinutes(Constants.AuthorExpirationTimeInMinutes));
            }
            return result;
        }

        public static async Task<ApiAuthor> Get(string token, string id, bool forceRefresh = false)
        {
            var authorKey = AUTHORS_SINGLE_KEY + id;
            ApiAuthor result = null;

            if (!forceRefresh && Barrel.Current.Exists(authorKey) && !Barrel.Current.IsExpired(authorKey))
                result = Barrel.Current.Get<ApiAuthor>(authorKey);

            if (result == null)
            {
                var api = RestService.For<IAuthorApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));

                result = await api.GetAuthor(id);
                Barrel.Current.Add(authorKey, result, TimeSpan.FromMinutes(Constants.AuthorExpirationTimeInMinutes));
            }

            return result;
        }

        #endregion

        #region SEARCH

        public static async Task<ApiAuthorResponse> Search(string token, string search, int? page = null)
        {
            ApiAuthorResponse result = null;

            var api = RestService.For<IAuthorApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            result = await api.SearchAuthors(new ApiSearchRequest(page, search));

            return result;
        }

        #endregion

        #region ADD

        public static async Task<ApiAuthor> Add(string token, ApiAuthorRequest author)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(AUTHORS_KEY);

            var api = RestService.For<IAuthorApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            return await api.AddAuthor(author);
        }

        #endregion

        #region PATCH

        public static async Task<ApiAuthor> Update(string token, string id, ApiAuthorRequest author)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(AUTHORS_KEY, AUTHORS_SINGLE_KEY, author.Id);

            var api = RestService.For<IAuthorApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            return await api.UpdateAuthor(author, id);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, string id)
        {
            //Clear cache
            ManagerUtils.ClearPageBarrels(AUTHORS_KEY, AUTHORS_SINGLE_KEY, id);

            var api = RestService.For<IAuthorApi>(HttpUtils.GetHttpClient(Constants.ThePage_Api_Url, token));
            await api.DeleteAuthor(id);

            return true;
        }

        #endregion
    }
}