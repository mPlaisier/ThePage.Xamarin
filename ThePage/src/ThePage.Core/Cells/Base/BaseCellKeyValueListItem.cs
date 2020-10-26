using System;
using MvvmCross.Commands;

namespace ThePage.Core
{
    public class BaseCellKeyValueListItem : BaseCellInput
    {
        readonly Action<ICell> _action;

        #region Properties

        public string Key { get; }

        public string Value { get; }

        public string Icon { get; }

        #endregion

        #region Commands

        IMvxCommand _commandClick;
        public IMvxCommand CommandClick => _commandClick ??= new MvxCommand(() => _action.Invoke(this));

        #endregion

        #region Constructor

        public BaseCellKeyValueListItem(string key, string value, Action<ICell> action, string icon = "ic_delete", bool isEdit = false)
        {
            Key = key;
            Value = value;
            _action = action;
            Icon = icon;
            IsEdit = isEdit;
        }

        #endregion
    }
}
