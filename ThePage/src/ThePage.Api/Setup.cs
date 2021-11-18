using MvvmCross.IoC;

namespace ThePage.Api
{
    public static class Setup
    {
        public static void Initialize()
        {
            typeof(Setup).Assembly.CreatableTypes().WithAttribute<ThePageLazySingletonServiceAttribute>().AsInterfaces().RegisterAsLazySingleton();
            typeof(Setup).Assembly.CreatableTypes().WithAttribute<ThePageTypeServiceAttribute>().AsInterfaces().RegisterAsDynamic();
        }
    }
}
