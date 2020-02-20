using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThePage.Core
{
    public interface IThePageService
    {
        Task<List<BookCell>> GetAllBooks();

        Task<List<AuthorCell>> GetAllAuthors();
    }
}
