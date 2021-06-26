using System;
using MvvmCross.ViewModels;

namespace ThePage.Core
{
    public abstract class CellBookInput : MvxNotifyPropertyChanged, ICellBook
    {
        public enum EBookInputType
        {
            BasicInfo,
            Title,
            Author,
            Genre,
            ISBN,
            Owned,
            Read,
            Pages
        }

        public Action UpdateValidation { get; internal set; }

        #region Properties

        bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public abstract bool IsValid { get; }

        public abstract EBookInputType InputType { get; }

        #endregion
    }
}
