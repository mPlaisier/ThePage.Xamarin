using MvvmCross.Platforms.Android.Presenters.Attributes;
using ThePage.Core.ViewModels.Main;

namespace ThePage.Droid.Views.Main
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel),
       Resource.Id.content_frame,
       AddToBackStack = false,
       EnterAnimation = Resource.Animation.pull_in_down,
       ExitAnimation = Resource.Animation.push_out_up,
       PopEnterAnimation = Resource.Animation.pull_in_up,
       PopExitAnimation = Resource.Animation.push_out_down
   )]
    public class MainFragment : BaseFragment<MainViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.fragment_main;

        protected override EToolbarIcon ToolbarIcon => EToolbarIcon.Logout;
    }
}
