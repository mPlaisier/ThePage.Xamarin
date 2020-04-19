namespace ThePage.Core
{
    public class CellBookTitle : ICellBook
    {
        #region Properties

        public string LblTitle { get; }

        #endregion

        #region Constructor

        public CellBookTitle(string lblTitle)
        {
            LblTitle = lblTitle;
        }

        #endregion
    }
}
