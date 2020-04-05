using ThePage.Api;

namespace ThePage.Core
{
    public class CellAuthor : CellBase
    {
        #region Properties

        public Author Author { get; }

        #endregion

        #region Constructor

        public CellAuthor()
        {

        }

        public CellAuthor(Author author)
        {
            Author = author;
        }

        #endregion
    }
}
