using System;
namespace ThePage.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ThePageLazySingletonServiceAttribute : Attribute
    {
    }
}