namespace ThePage.Core
{
    public class CellDebugHeader : BaseHeaderCell, ICellDebug
    {
        #region Properties

        public EDebugType DebugType { get; }

        #endregion

        #region Constructor

        public CellDebugHeader(string title, EDebugType debugType, bool isOpen = true)
        {
            Title = title;
            DebugType = debugType;
            IsOpen = isOpen;
        }

        #endregion
    }
}