using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace ThePage.Core
{
    public class CellBookButton : MvxNotifyPropertyChanged, ICellBook
    {
        Func<Task> _btnAction { get; }

        #region Properties

        public string Label => "Add Book";

        bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        #endregion

        #region Commands

        IMvxCommand _clickCommand;
        public IMvxCommand ClickCommand => _clickCommand ??= new MvxCommand(() => _btnAction.Invoke().Forget());

        #endregion

        #region Constructor

        public CellBookButton(Func<Task> btnAction)
        {
            _btnAction = btnAction;
        }

        #endregion
    }
}