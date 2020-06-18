using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ThePage.Core.ViewModels.Main;
using MvvmCross.Platforms.Android.Presenters.Attributes;

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

        protected override bool ShowNavigationIcon => false;
    }
}
