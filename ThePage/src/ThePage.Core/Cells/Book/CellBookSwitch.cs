using System;
namespace ThePage.Core
{
    public class CellBookSwitch : CellBookInput
    {
        #region Properties

        public string LblTitle { get; }

        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (SetProperty(ref _isSelected, value))
                    UpdateValidation?.Invoke();
            }
        }

        public override bool IsValid => true;

        public override EBookInputType InputType { get; }

        #endregion

        #region Constructor

        public CellBookSwitch(string lblTitle, EBookInputType inputType, Action updateValidation, bool isEdit = false)
        {
            LblTitle = lblTitle;
            InputType = inputType;
            UpdateValidation = updateValidation;
            IsEdit = isEdit;
        }

        public CellBookSwitch(string lblTitle, bool value, EBookInputType inputType, Action updateValidation, bool isEdit = false)
            : this(lblTitle, inputType, updateValidation, isEdit)
        {
            IsSelected = value;
        }

        #endregion
    }
}