using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThePage.Core
{
    public interface IAuthorService
    {
        #region Properties

        string SearchText { get; }

        bool IsSearching { get; }

        #endregion

        #region Methods

        Task<IEnumerable<Author>> GetAuthors();

        Task<IEnumerable<Author>> LoadNextAuthors();

        Task<IEnumerable<Author>> Search(string search);

        Task<Author> AddAuthor(string name);

        Task<Author> UpdateAuthor(Author author);

        #endregion
    }
}