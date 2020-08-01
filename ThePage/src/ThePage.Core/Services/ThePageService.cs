using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.LiteDB;
using MvvmCross;
using Refit;
using ThePage.Api;

namespace ThePage.Core
{
    public class ThePageService : IThePageService
    {
        readonly IUserInteraction _userInteraction;
        readonly IAuthService _authService;

        #region Constructor

        public ThePageService() :
            this(Mvx.IoCProvider.Resolve<IUserInteraction>(),
                 Mvx.IoCProvider.Resolve<IAuthService>())
        {
        }

        public ThePageService(IUserInteraction userInteraction, IAuthService authService)
        {
            _userInteraction = userInteraction;
            _authService = authService;

            Barrel.ApplicationId = "thepageapplication";
            Barrel.EncryptionKey = "encryptionKey";
        }

        #endregion

        #region Public(Books)

        public async Task<ApiBookResponse> GetAllBooks()
        {
            ApiBookResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Get(token);
            }
            catch (Exception ex)
            {
                HandleException(ex);

            }
            return result;
        }

        public async Task<ApiBookDetailResponse> GetBook(string id)
        {
            ApiBookDetailResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Get(token, id);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> AddBook(ApiBookDetailRequest book)
        {
            ApiBookDetailResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Add(token, book);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<ApiBookDetailResponse> UpdateBook(ApiBookDetailResponse book)
        {
            ApiBookDetailResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await BookManager.Update(token, book);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteBook(ApiBookDetailResponse content)
        {
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    return await BookManager.Delete(token, content);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        #endregion

        #region Authors

        public async Task<ApiAuthorResponse> GetAllAuthors()
        {
            ApiAuthorResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await AuthorManager.Get(token);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> AddAuthor(ApiAuthorRequest author)
        {
            ApiAuthor result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await AuthorManager.Add(token, author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<ApiAuthor> UpdateAuthor(ApiAuthor author)
        {
            ApiAuthor result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await AuthorManager.Update(token, author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteAuthor(ApiAuthor author)
        {
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    return await AuthorManager.Delete(token, author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        #endregion

        #region Genres

        public async Task<ApiGenreResponse> GetAllGenres()
        {
            ApiGenreResponse result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Get(token);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> AddGenre(ApiGenreRequest genre)
        {
            ApiGenre result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Add(token, genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<ApiGenre> UpdateGenre(ApiGenre genre)
        {
            ApiGenre result = null;
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    result = await GenreManager.Update(token, genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteGenre(ApiGenre genre)
        {
            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                    return await GenreManager.Delete(token, genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
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