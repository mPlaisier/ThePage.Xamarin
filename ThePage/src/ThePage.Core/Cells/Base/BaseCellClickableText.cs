using System;
using System.Threading.Tasks;
using MvvmCross.Commands;

namespace ThePage.Core
{
    public class BaseCellClickableText : ICell
    {
        readonly Func<Task> _action;

        #region Properties

        public string Label { get; }

        public string Icon { get; }

        #endregion

        #region Commands

        IMvxAsyncCommand _commandClick;
        public IMvxAsyncCommand CommandClick => _commandClick ??= new MvxAsyncCommand(_action);

        #endregion

        #region Constructor

        public BaseCellClickableText(string label, Func<Task> action, string icon = Constants.ICON_ADD)
        {
            Label = label;
            _action = action;
            Icon = icon;
        }

        #endregion

    }
}