using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using ThePage.Core;
using ThePage.Core.ViewModels.Main;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel),
        Resource.Id.content_frame,
        AddToBackStack = true,
        EnterAnimation = Resource.Animation.pull_in_right,
        ExitAnimation = Resource.Animation.push_out_left,
        PopEnterAnimation = Resource.Animation.pull_in_left,
        PopExitAnimation = Resource.Animation.push_out_right
    )]
    public class GenreDetailFragment : BaseFragment<GenreDetailViewModel>, ViewTreeObserver.IOnGlobalFocusChangeListener
    {
        #region Properties

        protected override int FragmentLayoutId => Resource.Layout.fragment_genredetail;

        IMvxInteraction _updateToolbar;
        public IMvxInteraction UpdateToolbarInteraction
        {
            get => _updateToolbar;
            set
            {
                if (_updateToolbar != null)
                    _updateToolbar.Requested -= OnUpdateToolbarRequested;

                _updateToolbar = value;
                if (_updateToolbar != null)
                    _updateToolbar.Requested += OnUpdateToolbarRequested;
            }
        }

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

        public override void OnViewModelSet()
        {
            base.OnViewModelSet();

            UpdateToolbarInteraction = ViewModel.UpdateToolbarInteraction;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            UpdateToolbarInteraction = null;
        }

        #endregion

        #region IOnGlobalFocusChangeListener

        public void OnGlobalFocusChanged(View oldFocus, View newFocus)
        {
            if (!(newFocus is EditText))
                DroidUtils.HideKeyboard(Activity);
        }

        #endregion

        #region Private

        void OnUpdateToolbarRequested(object sender, EventArgs e)
        {
            UpdateToolbar();
        }

        #endregion
    }
}