using static ThePage.Core.Enums;

namespace ThePage.Core.Cells
{
    public interface ICellBookInput : ICellBook
    {
        #region Properties

        bool IsEdit { get; set; }

        bool IsValid { get; }

        EBookInputType InputType { get; }

        #endregion
    }
}
