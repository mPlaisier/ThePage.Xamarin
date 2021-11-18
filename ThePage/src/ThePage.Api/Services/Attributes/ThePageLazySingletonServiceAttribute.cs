using System;
namespace ThePage.Api
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ThePageLazySingletonServiceAttribute : Attribute
    {
    }
}