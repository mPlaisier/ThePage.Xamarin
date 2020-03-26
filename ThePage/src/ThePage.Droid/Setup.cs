using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using ThePage.Core;

namespace ThePage.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<IUserInteraction, UserInteraction>();
        }

    }
}
