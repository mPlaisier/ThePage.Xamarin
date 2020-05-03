using System;
using System.Collections.Generic;
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

        #region Properties

        static readonly IAuthorAPI _authorApi = RestService.For<IAuthorAPI>(Secrets.ThePageAPI_URL);

        #endregion

        #region FETCH

        public static async Task<List<Author>> Get(bool forceRefresh = false)
        {
            List<Author> result = null;
            if (!forceRefresh && Barrel.Current.Exists(GetAuthorsKey) && !Barrel.Current.IsExpired(GetAuthorsKey))
                result = Barrel.Current.Get<List<Author>>(GetAuthorsKey);

            if (result == null)
            {
                result = await _authorApi.GetAuthors();
                Barrel.Current.Add(GetAuthorsKey, result, TimeSpan.FromMinutes(Constants.AuthorExpirationTimeInMinutes));
            }
            return result;
        }

        public static async Task<Author> Get(string id, bool forceRefresh = false)
        {
            var authorKey = GetSingleAuthorKey + id;
            Author result = null;

            if (!forceRefresh && Barrel.Current.Exists(authorKey) && !Barrel.Current.IsExpired(authorKey))
                result = Barrel.Current.Get<Author>(authorKey);

            if (result == null)
            {
                result = await _authorApi.GetAuthor(id);
                Barrel.Current.Add(authorKey, result, TimeSpan.FromMinutes(Constants.AuthorExpirationTimeInMinutes));
            }

            return result;
        }

        #endregion

        #region ADD

        public static async Task<Author> Add(Author author)
        {
            //Clear cache
            Barrel.Current.Empty(GetAuthorsKey);

            return await _authorApi.AddAuthor(author);
        }

        #endregion

        #region PATCH

        public static async Task<Author> Update(Author author)
        {
            //Clear cache
            Barrel.Current.Empty(GetAuthorsKey);

            return await _authorApi.UpdateAuthor(author);
        }

        #endregion

        #region DELETE

        public static async Task<bool> Delete(Author author)
        {
            //Clear cache
            Barrel.Current.Empty(GetAuthorsKey);

            //TODO improve the API to return a better result
            //ATM we receive if successfull:
            //"{\"message\":\"Deleted book\"}"
            await _authorApi.DeleteAuthor(author);

            return true;
        }

        #endregion
    }
}