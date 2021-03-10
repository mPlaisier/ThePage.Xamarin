using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using ThePage.Api;

namespace ThePage.Core
{
    public class OpenLibraryService : IOpenLibraryService
    {
        readonly IUserInteraction _userInteraction;

        #region Constructor

        public OpenLibraryService(IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
        }

        #endregion

        #region Public

        public async Task<OLObject> GetBookByISBN(string isbn)
        {
            OLObject result = null;

            try
            {
                result = await OpenLibraryManager.Get(isbn);
            }
            catch (KeyNotFoundException ex)
            {
                HandleKeyNotFoundException(ex, isbn);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        #endregion

        #region Private

        //TODO Move to General handle exception class
        void HandleException(Exception ex)
        {
            Crashes.TrackError(ex);

            _userInteraction.Alert(ex.Message, null, "Error");
        }

        void HandleKeyNotFoundException(KeyNotFoundException ex, string isbn)
        {
            Crashes.TrackError(ex);

            var message = $"{isbn} is not found in the library. Please add the book manually";
            _userInteraction.Alert(message, null, "Error");
        }

        #endregion
    }
}