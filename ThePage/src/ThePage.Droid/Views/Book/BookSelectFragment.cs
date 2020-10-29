using System.ComponentModel;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ThePage.Core;
using ThePage.Core.ViewModels.Main;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel),
        Resource.Id.content_frame,
        AddToBackStack = true,
        EnterAnimation = Resource.Animation.pull_in_down,
        ExitAnimation = Resource.Animation.push_out_up)]
    public class BookSelectFragment : BaseListFragment<BookSelectViewModel>
    {
        #region Properties

        protected override int FragmentLayoutId => Resource.Layout.fragment_book_select;
        protected override EToolbarIcon ToolbarIcon => EToolbarIcon.Close;

        #endregion

        #region Private

        protected override void OnScrollListener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_scrolllistener.BottomReached))
            {
                if (_scrolllistener.BottomReached)
                    ViewModel.LoadNextPage().Forget();
            }
        }

        #endregion
    }
}