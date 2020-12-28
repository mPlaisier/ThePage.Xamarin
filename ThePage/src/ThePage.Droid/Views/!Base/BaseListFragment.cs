using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    public abstract class BaseListFragment<TViewModel> : BaseFragment<TViewModel>, SearchView.IOnQueryTextListener, SearchView.IOnCloseListener
        where TViewModel : BaseListViewModel, IMvxViewModel
    {
        protected OnScrollListener _scrolllistener;

        #region LifeCycle

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.lstItems);
            if (recyclerView == null)
                throw new Exception($"Fragment {this} should have an recyclerview with id=lstItems");

            _scrolllistener = new OnScrollListener();
            recyclerView.AddOnScrollListener(_scrolllistener);

            HasOptionsMenu = true;

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

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_search, menu);

            var mSearchMenuItem = menu.FindItem(Resource.Id.search);
            var searchView = (SearchView)mSearchMenuItem.ActionView;

            searchView.SetOnQueryTextListener(this);
            searchView.SetOnCloseListener(this);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        #endregion

        #region Public

        public bool OnQueryTextSubmit(string newText)
        {
            ViewModel.Search(newText);
            return true;
        }

        public bool OnQueryTextChange(string newText)
        {
            if (newText.Equals(string.Empty))
                OnClose();

            return true;
        }

        public bool OnClose()
        {
            ViewModel.StopSearch();
            return false;
        }

        #endregion

        #region protected

        protected void OnScrollListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_scrolllistener.BottomReached))
            {
                if (_scrolllistener.BottomReached)
                    ViewModel.LoadNextPage();
            }
        }

        #endregion
    }
}
