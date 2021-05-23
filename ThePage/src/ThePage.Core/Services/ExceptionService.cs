using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace ThePage.Core
{
    public class ExceptionService : IExceptionService
    {
        bool _HaslogginEnabled = false;

        #region Properties

        public bool HasExceptions => Exceptions.IsNotNullAndHasItems();

        public List<ExceptionContainer> Exceptions { get; private set; } = new List<ExceptionContainer>();

        #endregion

        #region Public

        public void LogginIsEnabled()
        {
            _HaslogginEnabled = true;
            if (HasExceptions)
            {
                foreach (var error in Exceptions)
                {
                    Crashes.TrackError(error.Exception, error.ErrorData);
                }
                Exceptions = null;
            }
        }

        public void AddExceptionForLogging(Exception exception, Dictionary<string, string> exceptionData)
        {
            if (_HaslogginEnabled)
            {
                Crashes.TrackError(exception, exceptionData);
            }
            else
                Exceptions.Add(new ExceptionContainer(exception, exceptionData));
        }

        #endregion
    }

    public class ExceptionContainer
    {
        #region Properties

        public Exception Exception { get; }

        public Dictionary<string, string> ErrorData { get; }

        #endregion

        #region Constructor

        public ExceptionContainer(Exception exception, Dictionary<string, string> exceptionData)
        {
            Exception = exception;
            ErrorData = exceptionData;
        }

        #endregion
    }
}