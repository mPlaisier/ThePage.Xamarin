using System;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.LiteDB;
using Refit;

namespace ThePage.Core
{
    public partial class ThePageService : IThePageService
    {
        readonly IUserInteraction _userInteraction;
        readonly IAuthService _authService;

        #region Constructor

        public ThePageService(IUserInteraction userInteraction, IAuthService authService)
        {
            _userInteraction = userInteraction;
            _authService = authService;

            Barrel.ApplicationId = "thepageapplication";
            Barrel.EncryptionKey = "encryptionKey";
        }

        #endregion

        #region Private

        //TODO Move to General handle exception class
        void HandleException(Exception ex)
        {
            Crashes.TrackError(ex);

            if (ex is ApiException apiException)
            {
                if (apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _userInteraction.Alert("Item not found", null, "Error");
                }
            }
            else
            {
                _userInteraction.Alert(ex.Message, null, "Error");
            }
        }

        #endregion
    }
}