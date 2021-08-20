using MvvmCross.ViewModels;
using static ThePage.Core.Enums;

namespace ThePage.Core.Cells
{
    public abstract class CellBaseBookInput : MvxNotifyPropertyChanged, ICellBookInput
    {
        #region Properties

        public bool IsEdit { get; set; }

        public abstract bool IsValid { get; }

        public EBookInputType InputType { get; }

        #endregion

        #region Constructor

        protected CellBaseBookInput(EBookInputType inputType)
        {
            InputType = inputType;
        }

        #endregion
    }
}
