using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> result = null;
            try
            {
                result = await BookManager.Get();
                result = result.OrderBy(x => x.Title).ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);

            }
            return result.SortByTitle();
        }

        public async Task<Book> GetBook(string id)
        {
            Book result = null;
            try
            {
                result = await BookManager.Get(id);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> AddBook(Book book)
        {
            Book result = null;
            try
            {
                result = await BookManager.Add(book);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            Book result = null;
            try
            {
                result = await BookManager.Update(book);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteBook(Book content)
        {
            try
            {
                return await BookManager.Delete(content);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        public async Task<ApiBookResponse> GetAllBooksV2()
        {
            ApiBookResponse result = null;

            try
            {
                var token = await _authService.GetSessionToken();

                if (token != null)
                {
                    result = await BookManager.GetV2(token);
                }
                //TODO add logout feature when token == null
            }
            catch (Exception ex)
            {
                HandleException(ex);

            }
            return result;
        }

        #endregion

        #region Authors

        public async Task<List<Author>> GetAllAuthors()
        {
            List<Author> result = null;
            try
            {
                result = await AuthorManager.Get();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result.SortByName();
        }

        public async Task<bool> AddAuthor(Author author)
        {
            Author result = null;
            try
            {
                result = await AuthorManager.Add(author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<Author> UpdateAuthor(Author author)
        {
            Author result = null;
            try
            {
                result = await AuthorManager.Update(author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteAuthor(Author author)
        {
            try
            {
                return await AuthorManager.Delete(author);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        #endregion

        #region Genres

        public async Task<List<Genre>> GetAllGenres()
        {
            List<Genre> result = null;
            try
            {
                result = await GenreManager.Get();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result.SortByName();
        }

        public async Task<bool> AddGenre(Genre genre)
        {
            Genre result = null;
            try
            {
                result = await GenreManager.Add(genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result != null;
        }

        public async Task<Genre> UpdateGenre(Genre genre)
        {
            Genre result = null;
            try
            {
                result = await GenreManager.Update(genre);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> DeleteGenre(Genre genre)
        {
            try
            {
                return await GenreManager.Delete(genre);
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