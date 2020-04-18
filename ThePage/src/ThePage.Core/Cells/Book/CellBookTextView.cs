using System;
namespace ThePage.Core
{
    public class CellBookTextView : CellBookInput
    {
        bool _isRequired;

        #region Properties

        public string LblTitle { get; }

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

        public override bool IsValid => CheckValidation();

        public override EBookInputType InputType { get; }

        #endregion

        #region Constructor

        public CellBookTextView(string lblTitle, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = true)
        {
            LblTitle = lblTitle;
            InputType = inputType;
            UpdateValidation = updateValidation;
            _isRequired = isRequired;
            IsEdit = isEdit;
        }

        #endregion

        #region Private

        bool CheckValidation()
        {
            return !_isRequired ? true : !string.IsNullOrWhiteSpace(TxtInput);
        }

        #endregion
    }

    public class CellBookNumberTextView : CellBookTextView
    {
        #region Constructor

        public CellBookNumberTextView(string lblTitle, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = true)
            : base(lblTitle, inputType, updateValidation, isRequired, isEdit)
        {
        }

        #endregion
    }
}