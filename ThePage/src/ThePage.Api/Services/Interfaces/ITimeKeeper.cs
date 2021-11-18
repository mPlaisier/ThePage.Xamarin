using System;

namespace ThePage.Api
{
    public interface ITimeKeeper
    {
        DateTime Now { get; }
    }
}