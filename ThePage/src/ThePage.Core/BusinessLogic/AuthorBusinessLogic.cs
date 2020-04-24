using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class AuthorBusinessLogic
    {
        #region Public

        public static List<CellAuthor> AuthorsToCellAuthors(List<Author> authorsApi)
        {
            return authorsApi?.Select(author => new CellAuthor(author)).ToList();
        }

        public static Author GetAuthorFromString(string author, IEnumerable<Author> authors)
        {
            return authors?.FirstOrDefault(a => a.Id.Equals(author));
        }

        #endregion
    }
}
