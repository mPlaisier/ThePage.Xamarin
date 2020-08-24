using System;

namespace ThePage.Core.ViewModels.Main
{
    public class MainContainerViewModel : BaseViewModel
    {
        IUserInteraction _userInteraction;

        #region Properties

        public override string LblTitle => "";

        public bool IsLogOut { get; set; } = false;

        #endregion

        #region Constructor

        public MainContainerViewModel(IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
        }

        #endregion

        public void LogOutUser()
        {
            _userInteraction.Confirm("Do you want to logout?", () =>
            {
                _userInteraction.ToastMessage("Log off");
            });
        }
    }

    public class MainContainerNoToolBarViewModel : BaseViewModel
    {
        public override string LblTitle => "";
    }
}
