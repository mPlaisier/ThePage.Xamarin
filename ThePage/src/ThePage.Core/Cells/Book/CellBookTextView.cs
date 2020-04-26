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

        public int TxtNumberInput => ConvertToNumber();

        public override bool IsValid => CheckValidation();

        public override EBookInputType InputType { get; }

        #endregion

        #region Constructor

        public CellBookTextView(string lblTitle, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = false)
        {
            LblTitle = lblTitle;
            InputType = inputType;
            UpdateValidation = updateValidation;
            _isRequired = isRequired;
            IsEdit = isEdit;
        }

        public CellBookTextView(string lblTitle, string value, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : this(lblTitle, inputType, updateValidation, isRequired, isEdit)
        {
            TxtInput = value;
        }

        #endregion

        #region Private

        bool CheckValidation()
        {
            return !_isRequired ? true : !string.IsNullOrWhiteSpace(TxtInput);
        }

        int ConvertToNumber()
        {
            int.TryParse(TxtInput, out var number);
            return number;
        }

        #endregion
    }

    public class CellBookNumberTextView : CellBookTextView
    {
        #region Constructor

        public CellBookNumberTextView(string lblTitle, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : base(lblTitle, inputType, updateValidation, isRequired, isEdit)
        {
        }

        public CellBookNumberTextView(string lblTitle, string value, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : base(lblTitle, value, inputType, updateValidation, isRequired, isEdit)
        {
        }

        #endregion
    }
}