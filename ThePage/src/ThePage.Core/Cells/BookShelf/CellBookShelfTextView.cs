using System;
namespace ThePage.Core
{
    public class CellBookShelfTextView : BaseCellTextView, ICellBookShelfInput
    {
        #region Properties

        public EBookShelfInputType InputType { get; }

        #endregion

        #region MyRegion

        public CellBookShelfTextView(EBookShelfInputType inputType, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : base(updateValidation, isRequired, isEdit)
        {
            InputType = inputType;
        }

        public CellBookShelfTextView(EBookShelfInputType inputType, string value, Action updateValidation, bool isRequired = true, bool isEdit = false)
            : base(value, updateValidation, isRequired, isEdit)
        {
            InputType = inputType;
        }

        #endregion
    }

    public enum EBookShelfInputType
    {
        Name,
        Book
    }

    public interface ICellBookShelfInput { }
}
