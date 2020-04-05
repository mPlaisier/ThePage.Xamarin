using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class AuthorBusinessLogic
    {
        #region Public

        public static List<AuthorCell> AuthorsToAuthorCells(List<Author> authorsApi)
        {
            return authorsApi?.Select(author => new AuthorCell(author)).ToList();
        }

        #endregion
    }
}
