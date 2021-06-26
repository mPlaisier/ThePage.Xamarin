using System;
using MvvmCross.Commands;

namespace ThePage.Core
{
    public class BaseCellKeyValueListItem : BaseCellInput
    {
        readonly Action<ICell> _actionClick;
        readonly Action<ICell> _actionEditClick;

        #region Properties

        public string Key { get; }

        public string Value { get; }

        public string Icon { get; }

        #endregion

        #region Commands

        IMvxCommand _commandClick;
        public IMvxCommand CommandClick => _commandClick ??= new MvxCommand(() =>
        {
            if (IsEdit)
                _actionEditClick?.Invoke(this);
            else
                _actionClick?.Invoke(this);
        });

        #endregion

        #region Constructor

        public BaseCellKeyValueListItem(string key, string value,
                                        Action<ICell> actionEditClick, Action<ICell> actionClick = null,
                                        string icon = Constants.ICON_DELETE, bool isEdit = false)
        {
            Key = key;
            Value = value;
            _actionEditClick = actionEditClick;
            _actionClick = actionClick;
            Icon = icon;
            IsEdit = isEdit;
        }

        #endregion
    }
}
