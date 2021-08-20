using System;
using static ThePage.Core.Enums;

namespace ThePage.Core.Cells
{
    public abstract class CellBaseBookInputValidation : CellBaseBookInput, ICellValidation
    {
        #region Properties

        public Action UpdateValidation { get; }

        #endregion

        #region Constructor

        protected CellBaseBookInputValidation(Action updateValidation, EBookInputType inputType) : base(inputType)
        {
            UpdateValidation = updateValidation ?? throw new ArgumentNullException(nameof(updateValidation));
        }

        #endregion
    }
}
