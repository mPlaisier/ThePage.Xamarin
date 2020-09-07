using System;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ThePage.Core;
using ThePage.Core.ViewModels.Main;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame, AddToBackStack = true, EnterAnimation = Resource.Animation.pull_in_down, ExitAnimation = Resource.Animation.push_out_up)]
    public class SelectGenreFragment : BaseFragment<SelectGenreViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.fragment_selectgenre;
        protected override EToolbarIcon ToolbarIcon => EToolbarIcon.Close;
    }
}