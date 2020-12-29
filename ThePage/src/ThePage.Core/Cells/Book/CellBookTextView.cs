using System;
namespace ThePage.Core
{
    public class CellBookTextView : CellBookInput
    {
        protected bool _isRequired;

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
            return !_isRequired || !string.IsNullOrWhiteSpace(TxtInput);
        }

        #endregion
    }

    public class CellBookNumberTextView : CellBookTextView
    {
        #region Properties

        public int TxtNumberInput => ConvertToNumber();

        public override bool IsValid => CheckValidation();

        #endregion
        #region Constructor

        public CellBookNumberTextView(string lblTitle, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : base(lblTitle, inputType, updateValidation, isRequired, isEdit)
        {
            _isRequired = isRequired;
        }

        public CellBookNumberTextView(string lblTitle, string value, EBookInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : base(lblTitle, value, inputType, updateValidation, isRequired, isEdit)
        {
        }

        #endregion

        #region Private

        int ConvertToNumber()
        {
            var parseOk = int.TryParse(TxtInput, out var number);

            return parseOk ? number : -1;
        }

        bool CheckValidation()
        {
            return !_isRequired || TxtNumberInput > -1;
        }

        #endregion
    }
}