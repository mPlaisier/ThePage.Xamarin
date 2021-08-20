using System;

namespace ThePage.Core
{
    public interface IAddBookScreenManagerService : IBaseBookDetailScreenManager
    {
        #region Methods

        void Init(AddBookParameter parameter, Action<string> actionClose);

        #endregion
    }
}
