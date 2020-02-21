using MvvmCross.IoC;
using MvvmCross.ViewModels;
using ThePage.Core.ViewModels.Main;

namespace ThePage.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<AuthorViewModel>();
        }
    }
}
