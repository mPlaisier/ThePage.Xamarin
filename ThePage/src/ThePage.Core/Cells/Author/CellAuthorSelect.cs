using ThePage.Api;

namespace ThePage.Core
{
    public class CellAuthorSelect : ICellBaseSelect<ApiAuthor>
    {
        #region Properties

        public ApiAuthor Item { get; set; }

        public bool IsSelected { get; set; }

        public string IconSelected => "ic_check";

        #endregion

        #region Constructor

        public CellAuthorSelect(ApiAuthor author, bool isSelected = false)
        {
            Item = author;

            IsSelected = author != null ? isSelected : false;
        }

        #endregion
    }
}