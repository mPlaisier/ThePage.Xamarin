using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace ThePage.Core
{
    public class CellBookButton : MvxNotifyPropertyChanged, ICellBook
    {
        readonly bool _requireValidation;

        Func<Task> _btnAction { get; }

        #region Properties

        public string Label { get; }

        bool _isValid;
        public bool IsValid
        {
            get => _requireValidation ? _isValid : true;
            set => SetProperty(ref _isValid, value);
        }

        #endregion

        #region Commands

        IMvxCommand _clickCommand;
        public IMvxCommand ClickCommand => _clickCommand ??= new MvxCommand(() => _btnAction?.Invoke().Forget());

        #endregion

        #region Constructor

        public CellBookButton(string lblBtn, Func<Task> btnAction, bool requireValidation = true)
        {
            Label = lblBtn;
            _btnAction = btnAction;
            _requireValidation = requireValidation;
        }

        #endregion
    }
}