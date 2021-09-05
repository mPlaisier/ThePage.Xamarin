using System.Collections.Generic;
using System.Linq;
using Nelibur.ObjectMapper;
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
            TinyMapper.Bind<ApiAuthor, Author>();
            return TinyMapper.Map<Author>(author);
        }

        #endregion
    }
}
