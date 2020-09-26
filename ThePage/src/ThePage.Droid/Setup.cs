using System.Collections.Generic;
using System.Reflection;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using Serilog;
using ThePage.Core;

namespace ThePage.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        public override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Serilog;

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

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<TextView>("DrawableRight",
               view => new TextViewDrawableRightBinding(view));

            registry.RegisterCustomBindingFactory<TextView>("DrawableLeft",
               view => new TextViewDrawableLeftBinding(view));
        }

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .WriteTo.AndroidLog()
                                .CreateLogger();
            return base.CreateLogProvider();
        }
    }
}