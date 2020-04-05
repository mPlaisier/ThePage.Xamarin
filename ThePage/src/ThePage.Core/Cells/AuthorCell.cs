using ThePage.Api;

namespace ThePage.Core
{
    public class AuthorCell : BaseCell
    {
        #region Properties

        public Author Author { get; }

        #endregion

        #region Constructor

        public AuthorCell()
        {

        }

        public AuthorCell(Author author)
        {
            Author = author;
        }

        #endregion
    }
}
