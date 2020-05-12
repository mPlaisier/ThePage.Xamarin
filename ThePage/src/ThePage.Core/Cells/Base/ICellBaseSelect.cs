using System;
namespace ThePage.Core
{
    public interface ICellBaseSelect<TObject>
    {
        #region Properties

        public TObject Item { get; set; }

        public bool IsSelected { get; set; }

        public string IconSelected { get; }

        #endregion
    }
}