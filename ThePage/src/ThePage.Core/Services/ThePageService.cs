using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using Refit;
using ThePage.Api;

namespace ThePage.Core
{
    public class ThePageService : IThePageService
    {
        readonly IUserInteraction _userInteraction;
        #region Constructor

        public ThePageService() : this(Mvx.IoCProvider.Resolve<IUserInteraction>())
        {

        }

        public ThePageService(IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
        }

        #endregion
        #region Public(Books)

        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> result = null;
            try
            {
                result = await BookManager.FetchBooks();
            }
            catch (Exception ex)
            {
                HandleException(ex);

            }
            return result;
        }

        public async Task<Book> GetBook(string id)
        {
            Book result = null;
            try
            {
                result = await BookManager.FetchBook(id);
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
                result = await BookManager.AddBook(book);
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
                result = await BookManager.UpdateBook(book);
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
                return await BookManager.DeleteBook(content);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return false;
        }

        #endregion

        #region Authors

        public async Task<List<Author>> GetAllAuthors()
        {
            List<Author> result = null;
            try
            {
                result = await AuthorManager.FetchAuthors();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return result;
        }

        public async Task<bool> AddAuthor(Author author)
        {
            Author result = null;
            try
            {
                result = await AuthorManager.AddAuthor(author);
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
                result = await AuthorManager.UpdateAuthor(author);
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
                return await AuthorManager.DeleteAuthor(author);
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
            return result;
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

        //TODO perhaps move to a general Utils class/file
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