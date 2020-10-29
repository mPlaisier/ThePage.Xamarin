using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    public abstract class BaseListFragment<TViewModel> : BaseFragment<TViewModel>
        where TViewModel : BaseViewModel, IMvxViewModel
    {
        protected OnScrollListener _scrolllistener;

        #region LifeCycle

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            if (recyclerView == null)
                throw new Exception("Fragment should have an recyclerview with id=lstItems");

            _scrolllistener = new OnScrollListener();
            recyclerView.AddOnScrollListener(_scrolllistener);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            _scrolllistener.PropertyChanged += OnScrollListener_PropertyChanged;
        }

        public override void OnPause()
        {
            base.OnPause();

            _scrolllistener.PropertyChanged -= OnScrollListener_PropertyChanged;
        }

        #endregion

        #region Private

        protected abstract void OnScrollListener_PropertyChanged(object sender, PropertyChangedEventArgs e);

        #endregion
    }
}
