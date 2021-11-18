using System;
namespace ThePage.Api
{
    [ThePageLazySingletonService]
    public class TimeKeeper : ITimeKeeper
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
