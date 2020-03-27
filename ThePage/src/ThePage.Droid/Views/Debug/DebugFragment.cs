using System;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ThePage.Core;
using ThePage.Core.ViewModels.Main;
using ThePage.Droid.Views;

namespace ThePage.Droid
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame, AddToBackStack = true)]
    public class DebugFragment : BaseFragment<DebugViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.fragment_debug;
    }
}
