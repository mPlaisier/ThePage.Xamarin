using System;
namespace ThePage.Core.Cells
{
    public interface ICellValidation
    {
        #region Properties

        Action UpdateValidation { get; }

        #endregion
    }
}
