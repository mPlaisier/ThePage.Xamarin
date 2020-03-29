using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using ThePage.Api.Helpers;

namespace ThePage.Api
{
    public class AuthorManager
    {
        #region Properties

        static readonly IAuthorAPI _authorApi = RestService.For<IAuthorAPI>(Secrets.ThePageAPI_URL);

        #endregion

        #region FETCH

        public static async Task<List<Author>> FetchAuthors()
        {
            return await _authorApi.GetAuthors();
        }

        public static async Task<Author> FetchAuthor(string id)
        {
            return await _authorApi.GetAuthor(id);
        }

        #endregion

        #region ADD

        public static async Task<Author> AddAuthor(Author author)
        {
            return await _authorApi.AddAuthor(author);
        }

        #endregion

        #region PATCH

        public static async Task<Author> UpdateAuthor(Author author)
        {
            return await _authorApi.UpdateAuthor(author);
        }

        #endregion

        #region DELETE

        public static async Task<bool> DeleteAuthor(Author author)
        {
            //TODO improve the API to return a better result
            //ATM we receive if successfull:
            //"{\"message\":\"Deleted book\"}"
            await _authorApi.DeleteAuthor(author);

            return true;
        }

        #endregion
    }
}
