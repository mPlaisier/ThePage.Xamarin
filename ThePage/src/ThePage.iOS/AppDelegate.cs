using Foundation;
using MvvmCross.Platforms.Ios.Core;
using ThePage.Core;

namespace ThePage.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
    }
}
