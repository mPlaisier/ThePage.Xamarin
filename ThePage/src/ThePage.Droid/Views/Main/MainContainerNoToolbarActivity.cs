using Android.App;
using Android.OS;
using Android.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using ThePage.Core.ViewModels.Main;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    [Activity(
        NoHistory = true,
        Theme = "@style/AppTheme",
        WindowSoftInputMode = SoftInput.AdjustResize | SoftInput.StateHidden)]
    public class MainContainerNoToolbarActivity : BaseActivity<MainContainerNoToolBarViewModel>
    {
        #region Properties

        protected override int ActivityLayoutId => Resource.Layout.activity_main_no_toolbar_container;

        #endregion

        #region LifeCycle

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
#if DEBUG
            AppCenter.Start("2a35d66f-7eb1-4a30-af70-44348de2e4b9", typeof(Analytics), typeof(Crashes), typeof(Distribute));
#else
            AppCenter.Start("1196e785-1345-401c-b733-583d7dd3afa0", typeof(Analytics), typeof(Crashes), typeof(Distribute));
#endif
        }

        #endregion
    }
}
