using MvvmCross;
using MvvmCross.Platforms.Android;
using ThePage.Core;

namespace ThePage.Droid
{
    public class Device : IDevice
    {
        public void HideKeyboard()
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            DroidUtils.HideKeyboard(activity);
        }
    }
}
