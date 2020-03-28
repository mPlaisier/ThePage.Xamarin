using System;
namespace ThePage.Core
{
    public class CellDebugItem : CellDebug
    {
        #region Properties

        public string Label { get; }

        public EDebugType Type { get; }

        public EDebugItemType ItemType { get; }

        #endregion

        #region Constructor

        public CellDebugItem(string label, EDebugType type, EDebugItemType itemType)
        {
            Label = label;
            Type = type;
            ItemType = itemType;
        }

        #endregion
    }

    public enum EDebugItemType
    {
        ConfirmOk,
        ConfirmAnswer,
        ConfirmAsync,
        Alert,
        AlerAsync,
        InputOk,
        InputAnwser,
        InputAsync,
        ConfirmThreeButtons,
        ConfirmThreeButtonsAsync,
        Toast,
        BookNotFound
    }
}