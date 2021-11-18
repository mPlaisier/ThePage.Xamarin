using System.Collections.Generic;
using System.Linq;
using CBP.Extensions;
using ThePage.Api;

namespace ThePage.Core
{
    public static class AuthorBusinessLogic
    {
        #region Public static

        public static IEnumerable<Author> ConvertApiAuthorsToAuthors(IEnumerable<ApiAuthor> apiAuthors)
        {
            return apiAuthors.Select(author => MapAuthor(author));
        }

        public static Author MapAuthor(ApiAuthor author)
        {
            if (author.IsNull())
                return default;

            return new Author()
            {
                Id = author.Id,
                Name = author.Name,
                Olkey = author.Olkey
            };
        }

        #endregion
    }
}
