using System;
namespace ThePage.Core
{
    public class CellBookTitle : CellBookInput
    {
        #region Properties

        public string LblTitle => "Title:";

        string _txtTitle;
        public string TxtTitle
        {
            get => _txtTitle;
            set
            {
                if (SetProperty(ref _txtTitle, value))
                {
                    UpdateValidation?.Invoke();
                }
            }
        }

        public override bool IsValid => !string.IsNullOrWhiteSpace(TxtTitle);

        public override EBookInputType InputType => EBookInputType.Title;

        #endregion

        #region Constructor

        public CellBookTitle(Action updateValidation, bool isEdit = true)
        {
            UpdateValidation = updateValidation;
            IsEdit = isEdit;
        }

        #endregion
    }
}
