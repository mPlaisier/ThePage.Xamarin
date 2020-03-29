using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ThePage.Core;
using ThePage.Core.ViewModels.Main;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame, addToBackStack: true)]
    public class AddGenreFragment : BaseFragment<AddGenreViewModel>, ViewTreeObserver.IOnGlobalFocusChangeListener
    {
        #region Properties

        protected override int FragmentLayoutId => Resource.Layout.fragment_addgenre;

        #endregion

        #region LifeCycle

        public override void OnResume()
        {
            base.OnResume();
            View.ViewTreeObserver.AddOnGlobalFocusChangeListener(this);
        }

        public override void OnPause()
        {
            base.OnPause();
            View.ViewTreeObserver.RemoveOnGlobalFocusChangeListener(this);
        }

        #endregion

        #region IOnGlobalFocusChangeListener

        public void OnGlobalFocusChanged(View oldFocus, View newFocus)
        {
            if (!(newFocus is EditText))
                DroidUtils.HideKeyboard(Activity);
        }

        #endregion
    }
}