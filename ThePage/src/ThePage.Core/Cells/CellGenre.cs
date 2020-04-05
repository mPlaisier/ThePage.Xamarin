using ThePage.Api;

namespace ThePage.Core
{
    public class CellGenre
    {
        #region Properties

        public Genre Genre { get; }

        #endregion

        #region Constructor

        public CellGenre()
        {

        }

        public CellGenre(Genre genre)
        {
            Genre = genre;
        }

        #endregion
    }
}
