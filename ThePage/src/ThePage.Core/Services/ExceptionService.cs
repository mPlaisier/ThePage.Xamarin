using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Refit;
using ThePage.Api;

namespace ThePage.Core
{
    public class ExceptionService : IExceptionService
    {
        readonly IUserInteraction _userInteraction;

        bool _HaslogginEnabled = false;

        #region Properties

        public bool HasExceptions => Exceptions.IsNotNullAndHasItems();

        public List<ExceptionContainer> Exceptions { get; private set; } = new List<ExceptionContainer>();

        #endregion

        #region Constructor

        public ExceptionService(IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
        }

        #endregion

        #region Public

        //Enable Exception logging after AppCenter.Start
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

        /// <summary>
        /// //Add Exception to the logger. If logging is possible it will be send to AppCenter
        /// else it will be send out once logging is enabled.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="exceptionData"></param>
        public void AddExceptionForLogging(Exception exception, Dictionary<string, string> exceptionData)
        {
            if (_HaslogginEnabled)
            {
                Crashes.TrackError(exception, exceptionData);
            }
            else
                Exceptions.Add(new ExceptionContainer(exception, exceptionData));
        }

        public void HandleAuthException(Exception exception, string requestType)
        {
            var data = new Dictionary<string, string>
            {
                { "Service", nameof(AuthService) },
                { "RequestType", requestType }
            };

            if (exception is ApiException apiException)
            {
                ApiError error = JsonConvert.DeserializeObject<ApiError>(apiException.Content);
                if (apiException.StatusCode == HttpStatusCode.NotFound)
                {
                    _userInteraction.Alert("Item not found", null, "Error");
                }
                else if (apiException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _userInteraction.ToastMessage(error.Message, EToastType.Error);
                }
                else
                {
                    data.Add("StatusCode", apiException.StatusCode.ToString());

                    AddExceptionForLogging(exception, data);
                    _userInteraction.Alert(error.Message, null, "Error");
                }
            }
            else
            {
                AddExceptionForLogging(exception, data);
                _userInteraction.Alert(exception.Message, null, "Error");
            }
        }

        public void HandleOpenLibraryException(Exception exception, string requestType, string isbn)
        {
            var data = new Dictionary<string, string>
            {
                { "Service", nameof(OpenLibraryService) },
                { "RequestType", requestType },
                { "Isbn", isbn }
            };
            AddExceptionForLogging(exception, data);

            if (exception is KeyNotFoundException)
            {
                var message = $"{isbn} is not found in the library. Please add the book manually";
                _userInteraction.Alert(message, null, "Not found");
            }
            else
                _userInteraction.Alert(exception.Message, null, "Error");
        }

        public void HandleThePageException(Exception exception, string requestType)
        {
            var data = new Dictionary<string, string>
            {
                { "Service", nameof(ThePageService) },
                { "RequestType", requestType }
            };
            AddExceptionForLogging(exception, data);

            if (exception is ApiException apiException)
            {
                if (apiException.StatusCode == HttpStatusCode.NotFound)
                {
                    _userInteraction.Alert("Item not found", null, "Not Found");
                }
            }
            else
            {
                _userInteraction.Alert(exception.Message, null, "Error");
            }
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