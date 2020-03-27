using System;
namespace ThePage.Core
{
    public class CellDebugHeader : CellDebug
    {
        #region Properties

        public string Title { get; }

        public EDebugType DebugType { get; }

        public bool IsOpen { get; }

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
