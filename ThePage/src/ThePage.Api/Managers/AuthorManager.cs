using System;
using System.Threading.Tasks;
using MonkeyCache.LiteDB;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class AuthorManager
    {
        const string GetAuthorsKey = "GetAuthorsKey";
        const string GetSingleAuthorKey = "GetAuthorKey";

        #region FETCH

        public static async Task<ApiAuthorResponse> Get(string token, bool forceRefresh = false)
        {
            ApiAuthorResponse result = null;
            if (!forceRefresh && Barrel.Current.Exists(GetAuthorsKey) && !Barrel.Current.IsExpired(GetAuthorsKey))
                result = Barrel.Current.Get<ApiAuthorResponse>(GetAuthorsKey);

            if (result == null)
            {
                var api = RestService.For<IAuthorAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));

                result = await api.GetAuthors();
                Barrel.Current.Add(GetAuthorsKey, result, TimeSpan.FromMinutes(Constants.AuthorExpirationTimeInMinutes));
            }
            return result;
        }

        public static async Task<ApiAuthor> Get(string token, string id, bool forceRefresh = false)
        {
            var authorKey = GetSingleAuthorKey + id;
            ApiAuthor result = null;

            if (!forceRefresh && Barrel.Current.Exists(authorKey) && !Barrel.Current.IsExpired(authorKey))
                result = Barrel.Current.Get<ApiAuthor>(authorKey);

            if (result == null)
            {
                var api = RestService.For<IAuthorAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));

                result = await api.GetAuthor(id);
                Barrel.Current.Add(authorKey, result, TimeSpan.FromMinutes(Constants.AuthorExpirationTimeInMinutes));
            }

            return result;
        }

        #endregion

        #region ADD

        public static async Task<ApiAuthor> Add(string token, ApiAuthorRequest author)
        {
            //Clear cache
            Barrel.Current.Empty(GetAuthorsKey);

            var api = RestService.For<IAuthorAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.AddAuthor(author);
        }

        #endregion

        #region PATCH

        public static async Task<ApiAuthor> Update(string token, string id, ApiAuthorRequest author)
        {
            //Clear cache
            var authorKey = GetSingleAuthorKey + id;
            Barrel.Current.Empty(authorKey);
            Barrel.Current.Empty(GetAuthorsKey);

            var api = RestService.For<IAuthorAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            return await api.UpdateAuthor(author, id);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(string token, ApiAuthor author)
        {
            //Clear cache
            var authorKey = GetSingleAuthorKey + author.Id;
            Barrel.Current.Empty(authorKey);
            Barrel.Current.Empty(GetAuthorsKey);

            var api = RestService.For<IAuthorAPI>(HttpUtils.GetHttpClient(Secrets.ThePageAPI_URL, token));
            await api.DeleteAuthor(author);

            return true;
        }

        #endregion
    }
}