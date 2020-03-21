using System;
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
            return authorsApi.Select(x => new AuthorCell(x.Id, x.Name)).ToList();
        }

        public static Author AuthorCellToAuthor(AuthorCell authorCell)
        {
            return new Author(authorCell.Id, authorCell.Name);
        }

        #endregion
    }
}
