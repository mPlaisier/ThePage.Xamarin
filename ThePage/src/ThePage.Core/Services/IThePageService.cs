using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IThePageService
    {
        Task<List<BookCell>> GetAllBooks();

        Task<List<AuthorCell>> GetAllAuthors();

        Task<bool> AddAuthor(Author author);
    }
}
