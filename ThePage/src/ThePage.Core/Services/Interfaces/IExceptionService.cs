using System;
using System.Collections.Generic;

namespace ThePage.Core
{
    public interface IExceptionService
    {
        void LogginIsEnabled();

        void AddExceptionForLogging(Exception exception, Dictionary<string, string> exceptionData);

        void HandleAuthException(Exception exception, string requestType);

        void HandleOpenLibraryException(Exception exception, string requestType, string isbn);

        void HandleThePageException(Exception exception, string requestType);
    }
}