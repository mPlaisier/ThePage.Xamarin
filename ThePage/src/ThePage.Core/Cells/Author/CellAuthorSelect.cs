namespace ThePage.Core
{
    public class CellAuthorSelect : ICellBaseSelect<Author>
    {
        #region Properties

        public Author Item { get; set; }

        public bool IsSelected { get; set; }

        public string IconSelected => Constants.ICON_CHECK;

        #endregion

        #region Constructor

        public CellAuthorSelect(Author author, bool isSelected = false)
        {
            Item = author;
            IsSelected = author != null && isSelected;
        }

        #endregion
    }
}