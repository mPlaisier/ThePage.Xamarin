﻿using System.Collections.Generic;
using System.Reflection;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using Serilog;
using ThePage.Core;

namespace ThePage.Droid
{
    public class Setup : MvxAndroidSetup<App>
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
            typeof(MvxRecyclerView).Assembly
        };

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<TextView>("DrawableRight",
               view => new TextViewDrawableRightBinding(view));

            registry.RegisterCustomBindingFactory<TextView>("DrawableLeft",
               view => new TextViewDrawableLeftBinding(view));

            registry.RegisterCustomBindingFactory<ImageView>("Url",
                view => new ImageViewUrlBinding(view));
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