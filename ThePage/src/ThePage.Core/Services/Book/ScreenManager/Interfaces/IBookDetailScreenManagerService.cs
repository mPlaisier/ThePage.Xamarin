using System;

namespace ThePage.Core
{
    public interface IBookDetailScreenManagerService : IBaseBookDetailScreenManager
    {
        #region Properties

        string Title { get; }

        bool IsEditing { get; }

        #endregion

        #region Methods

        void Init(BookDetailParameter parameter, Action close);

        void ToggleEditValue();

        #endregion
    }
}
