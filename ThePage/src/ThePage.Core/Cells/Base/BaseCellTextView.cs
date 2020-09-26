using System;
using MvvmCross.ViewModels;

namespace ThePage.Core
{
    public class BaseCellTextView : BaseCellInput
    {
        protected bool _isRequired;

        #region Properties

        public override bool IsValid => CheckValidation();

        string _txtInput;
        public string TxtInput
        {
            get => _txtInput;
            set
            {
                if (SetProperty(ref _txtInput, value))
                    UpdateValidation?.Invoke();
            }
        }

        #region Constructor

        public BaseCellTextView(Action updateValidation, bool isRequired = true, bool isEdit = false)
        {
            UpdateValidation = updateValidation;
            _isRequired = isRequired;
            IsEdit = isEdit;
        }

        public BaseCellTextView(string value, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : this(updateValidation, isRequired, isEdit)
        {
            TxtInput = value;
        }

        #endregion

        #endregion

        #region Private

        bool CheckValidation()
        {
            return !_isRequired || !string.IsNullOrWhiteSpace(TxtInput);
        }

        #endregion
    }

    public abstract class BaseCellInput : MvxNotifyPropertyChanged, ICell
    {
        public Action UpdateValidation { get; internal set; }

        #region Properties

        public bool IsEdit { get; set; }

        public virtual bool IsValid { get; } = true;

        #endregion
    }
}
