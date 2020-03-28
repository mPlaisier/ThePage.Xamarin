using Android.App;
using Android.OS;
using Android.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ThePage.Core.ViewModels.Main;

namespace ThePage.Droid.Views.Main
{
    [Activity(
        Theme = "@style/AppTheme",
        WindowSoftInputMode = SoftInput.AdjustResize | SoftInput.StateHidden)]
    public class MainContainerActivity : BaseActivity<MainContainerViewModel>
    {
        #region Properties

        protected override int ActivityLayoutId => Resource.Layout.activity_main_container;

        #endregion

        #region LifeCycle

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AppCenter.Start("2a35d66f-7eb1-4a30-af70-44348de2e4b9",
                      typeof(Analytics), typeof(Crashes));
        }

        #endregion

        #region Public

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion
    }
}
