using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Graphics;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;
using ThePage.Droid.Views.Main;

namespace ThePage.Droid.Views
{
    public enum EToolbarIcon
    {
        Logout,
        Back,
        Close
    }

    public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel>
        where TViewModel : BaseViewModel, IMvxViewModel
    {
        protected abstract int FragmentLayoutId { get; }

        protected virtual bool ShowNavigationIcon => true;
        protected virtual EToolbarIcon ToolbarIcon => EToolbarIcon.Back;
        protected virtual bool ShowToolbar => true;

        protected Toolbar _toolbar;

        #region LifeCycle

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            InitializeToolbar();

            return this.BindingInflate(FragmentLayoutId, container, false);
        }

        #endregion

        #region Public

        public void InitializeToolbar()
        {
            if (Activity is MainContainerActivity activity)
            {
                if (ShowToolbar)
                {
                    _toolbar = activity.FindViewById<Toolbar>(Resource.Id.toolbar);

                    if (_toolbar != null)
                    {
                        var toolbarLayout = activity.FindViewById<View>(Resource.Id.layout_toolbar);
                        toolbarLayout.Visibility = ViewStates.Visible;

                        activity.SetSupportActionBar(_toolbar);

                        if (ShowNavigationIcon)
                        {
                            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                            activity.SupportActionBar.SetHomeAsUpIndicator(GetDrawableForToolBar(ToolbarIcon));

                            activity.ViewModel.IsLogOut = ToolbarIcon == EToolbarIcon.Logout;
                        }
                        else
                            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(false);

                        _toolbar.Title = ViewModel.LblTitle;
                    }
                }
                else
                {
                    var toolbarLayout = activity.FindViewById<View>(Resource.Id.layout_toolbar);
                    toolbarLayout.Visibility = ViewStates.Gone;
                }

            }
        }

        public void UpdateToolbar()
        {
            if (_toolbar != null)
                _toolbar.Title = ViewModel.LblTitle;
        }

        #endregion

        #region Prvate

        Drawable GetDrawableForToolBar(EToolbarIcon type)
        {
            Drawable icon;
            switch (type)
            {
                case EToolbarIcon.Logout:
                    icon = ResourcesCompat.GetDrawable(Resources, Resource.Drawable.ic_logout, null);
                    break;
                case EToolbarIcon.Close:
                    icon = ResourcesCompat.GetDrawable(Resources, Resource.Drawable.ic_close, null);
                    break;
                case EToolbarIcon.Back:
                default:
                    icon = ResourcesCompat.GetDrawable(Resources, Resource.Drawable.ic_arrow_back_toolbar, null);
                    break;

            }
            var color = new Color(ContextCompat.GetColor(Activity, Resource.Color.white));
            icon.SetColorFilter(BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(color, BlendModeCompat.SrcIn));

            return icon;
        }

        #endregion
    }
}