using System;
using static ThePage.Core.Enums;

namespace ThePage.Core.Cells
{
    public class CellBookSwitch : CellBaseBookInputValidation
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

        #endregion

        #region Constructor

        public CellBookSwitch(string lblTitle, EBookInputType inputType, Action updateValidation, bool isEdit = false)
            : base(updateValidation, inputType)
        {
            LblTitle = lblTitle;

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