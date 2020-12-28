using MvvmCross.Platforms.Android.Presenters.Attributes;
using ThePage.Core;
using ThePage.Core.ViewModels.Main;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame, AddToBackStack = true, EnterAnimation = Resource.Animation.pull_in_down, ExitAnimation = Resource.Animation.push_out_up)]
    public class AuthorSelectFragment : BaseListFragment<AuthorSelectViewModel>
    {
        #region Properties

        protected override int FragmentLayoutId => Resource.Layout.fragment_author_select;
        protected override EToolbarIcon ToolbarIcon => EToolbarIcon.Close;

        #endregion
    }
}