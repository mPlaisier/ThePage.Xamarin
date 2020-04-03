using System.Collections.Generic;
using System.Reflection;
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
            Mvx.IoCProvider.RegisterType<IDevice, Device>();
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView).Assembly
        };
    }
}
