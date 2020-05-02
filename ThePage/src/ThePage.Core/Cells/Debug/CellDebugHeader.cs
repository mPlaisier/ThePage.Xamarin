using System;
using MvvmCross.Commands;

namespace ThePage.Core
{
    public class CellDebugHeader : CellDebug
    {
        Action<EDebugType, bool> _handleHeaderClick;

        #region Properties

        public string Title { get; }

        public EDebugType DebugType { get; }

        public bool IsOpen { get; internal set; }

        #endregion

        #region Commands

        MvxCommand _itemClickCommand;
        public MvxCommand ItemClickCommand => _itemClickCommand = _itemClickCommand ?? new MvxCommand(() =>
        {
            _handleHeaderClick?.Invoke(DebugType, IsOpen);
            IsOpen = !IsOpen;
        });

        #endregion

        #region Constructor

        public CellDebugHeader(string title, EDebugType debugType, Action<EDebugType, bool> handleHeaderClick, bool isOpen = true)
        {
            Title = title;
            DebugType = debugType;
            _handleHeaderClick = handleHeaderClick;
            IsOpen = isOpen;
        }

        #endregion
    }
}
