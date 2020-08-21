using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;
using ThePage.Droid.Views.Main;

namespace ThePage.Droid.Views
{
    public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel>
        where TViewModel : BaseViewModel, IMvxViewModel
    {
        protected abstract int FragmentLayoutId { get; }
        protected virtual bool ShowNavigationIcon => true;

        Toolbar _toolbar;

        #region LifeCycle

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            InitializeToolbar();

            return this.BindingInflate(FragmentLayoutId, container, false);
        }

        #endregion

        #region Virtual

        public virtual bool OnBackPressed()
        {
            return false;
        }

        #endregion

        #region Public

        public void InitializeToolbar()
        {
            if (Activity is MainContainerActivity activity)
            {
                _toolbar = activity.FindViewById<Toolbar>(Resource.Id.toolbar);

                if (_toolbar != null)
                {
                    activity.SetSupportActionBar(_toolbar);

                    if (ShowNavigationIcon)
                        activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    else
                        activity.SupportActionBar.SetDisplayHomeAsUpEnabled(false);

                    _toolbar.Title = ViewModel.LblTitle;
                }
            }
        }

        #endregion
    }
}