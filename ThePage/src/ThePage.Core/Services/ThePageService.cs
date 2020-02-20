using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public class ThePageService : IThePageService
    {
        #region Books

        public async Task<List<BookCell>> GetAllBooks()
        {
            var books = await FetchBooks();
            var authors = await FetchAuthors();

            var list = new List<BookCell>();
            foreach (var item in books)
            {
                var author = authors.Where(x => x.Id == item.Author).First();
                list.Add(new BookCell(item.Id, item.Title, author));
            }
            return list;
        }

        #endregion

        #region Authors

        public async Task<List<AuthorCell>> GetAllAuthors()
        {
            var authors = await FetchAuthors();

            var list = new List<AuthorCell>();
            foreach (var item in authors)
            {
                list.Add(new AuthorCell(item.Id, item.Name));
            }
            return list;
        }

        #endregion

        #region Private

        async Task<List<Book>> FetchBooks()
        {
            return await BookManager.FetchBooks(CancellationToken.None);
        }

        async Task<List<Author>> FetchAuthors()
        {
            return await AuthorManager.FetchAuthors(CancellationToken.None);
        }


        #endregion
    }
}
