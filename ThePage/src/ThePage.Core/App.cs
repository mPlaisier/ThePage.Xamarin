using MvvmCross.IoC;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Setup.Initialize();

            CreatableTypes().WithAttribute<ThePageLazySingletonServiceAttribute>().AsInterfaces().RegisterAsLazySingleton();
            CreatableTypes().WithAttribute<ThePageTypeServiceAttribute>().AsInterfaces().RegisterAsDynamic();

            RegisterCustomAppStart<CustomAppStart>();
        }
    }
}