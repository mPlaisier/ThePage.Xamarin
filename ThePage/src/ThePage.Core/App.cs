using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace ThePage.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes().WithAttribute<ThePageLazySingletonServiceAttribute>().AsInterfaces().RegisterAsLazySingleton();
            CreatableTypes().WithAttribute<ThePageTypeServiceAttribute>().AsInterfaces().RegisterAsDynamic();

            RegisterCustomAppStart<CustomAppStart>();
        }
    }
}