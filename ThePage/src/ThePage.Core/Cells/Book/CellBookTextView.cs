using System;
using System.Threading.Tasks;
using CBP.Extensions;
using MvvmCross.Commands;

namespace ThePage.Core
{
    public class CellBookTextView : CellBookInput
    {
        protected bool _isRequired;
        protected Func<string, Task> _searchFunc;

        #region Properties

        public string LblTitle { get; }

        protected string _txtInput;
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

        public bool HasSearch => _searchFunc.IsNotNull() && IsEdit;

        #endregion

        #region Commands

        IMvxAsyncCommand _commandSearchClick;
        public IMvxAsyncCommand CommandSearchClick => _commandSearchClick ??= new MvxAsyncCommand(HandleSearch);

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

        protected virtual async Task HandleSearch()
        {
            if (!string.IsNullOrWhiteSpace(TxtInput))
                await _searchFunc(TxtInput);
        }

        #endregion

        public class TextViewBuilder
        {
            protected readonly CellBookTextView _cellBookTextView;

            #region Public

            public TextViewBuilder SetValue(string value)
            {
                _cellBookTextView._txtInput = value;
                return this;
            }

            public TextViewBuilder NotRequired()
            {
                _cellBookTextView._isRequired = false;
                return this;
            }

            public TextViewBuilder IsEdit()
            {
                _cellBookTextView.IsEdit = true;
                return this;
            }

            public TextViewBuilder AllowSearch(Func<string, Task> searchFunc)
            {
                _cellBookTextView._searchFunc = searchFunc;
                return this;
            }

            public CellBookTextView Build()
            {
                return _cellBookTextView;
            }

            #endregion

            #region Constructor

            public TextViewBuilder(string lblTitle, EBookInputType inputType, Action updateValidation)
            {
                _cellBookTextView = new CellBookTextView(lblTitle, inputType, updateValidation);
            }

            #endregion
        }
    }

    public class CellBookNumberTextView : CellBookTextView
    {
        #region Properties

        public long TxtNumberInput => ConvertToNumber();

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

        long ConvertToNumber()
        {
            var parseOk = long.TryParse(TxtInput, out var number);

            return parseOk ? number : -1;
        }

        bool CheckValidation()
        {
            return !_isRequired || TxtNumberInput > -1;
        }

        protected override async Task HandleSearch()
        {
            if (TxtNumberInput > -1)
                await _searchFunc(TxtNumberInput.ToString());
        }

        #endregion

        public class NumberTextViewBuilder
        {
            protected readonly CellBookNumberTextView _cellBookNumberTextView;

            #region Public

            public NumberTextViewBuilder SetValue(string value)
            {
                _cellBookNumberTextView._txtInput = value;
                return this;
            }

            public NumberTextViewBuilder NotRequired()
            {
                _cellBookNumberTextView._isRequired = false;
                return this;
            }

            public NumberTextViewBuilder IsEdit()
            {
                _cellBookNumberTextView.IsEdit = true;
                return this;
            }

            public NumberTextViewBuilder AllowSearch(Func<string, Task> searchFunc)
            {
                _cellBookNumberTextView._searchFunc = searchFunc;
                return this;
            }

            public CellBookTextView Build()
            {
                return _cellBookNumberTextView;
            }

            #endregion

            #region Constructor

            public NumberTextViewBuilder(string lblTitle, EBookInputType inputType, Action updateValidation)
            {
                _cellBookNumberTextView = new CellBookNumberTextView(lblTitle, inputType, updateValidation);
            }

            #endregion
        }
    }
}