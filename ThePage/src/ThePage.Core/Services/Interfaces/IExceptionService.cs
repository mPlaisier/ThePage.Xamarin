using System;
using System.Collections.Generic;

namespace ThePage.Core
{
    public interface IExceptionService
    {
        void LogginIsEnabled();

        void AddExceptionForLogging(Exception exception, Dictionary<string, string> exceptionData);
    }
}
