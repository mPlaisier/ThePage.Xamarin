using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThePage.Core
{
    public interface IGenreService
    {
        #region Properties

        string SearchText { get; }

        bool IsSearching { get; }

        #endregion

        #region Methods

        Task<IEnumerable<Genre>> GetGenres();

        Task<Genre> GetGenre(string id);

        Task<IEnumerable<Genre>> LoadNextGenres();

        Task<IEnumerable<Genre>> Search(string input);

        Task<Genre> AddGenre(string name);

        Task<bool> DeleteGenre(string id);

        #endregion
    }
}