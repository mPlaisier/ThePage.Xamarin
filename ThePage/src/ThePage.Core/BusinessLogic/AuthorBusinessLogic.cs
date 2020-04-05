using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class AuthorBusinessLogic
    {
        #region Public

        public static List<CellAuthor> AuthorsToAuthorCells(List<Author> authorsApi)
        {
            return authorsApi?.Select(author => new CellAuthor(author)).ToList();
        }

        #endregion
    }
}
