namespace ThePage.Core
{
    public class BaseCellTitle : ICell
    {
        #region Properties

        public string Title { get; }

        #endregion

        #region Constructor

        public BaseCellTitle(string title)
        {
            Title = title;
        }

        #endregion
    }

    public interface ICell
    {
    }
}
