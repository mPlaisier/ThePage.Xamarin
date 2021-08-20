using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IBookShelfService
    {
        #region Properties

        string SearchText { get; }

        bool IsSearching { get; }

        #endregion

        #region Methods

        Task<IEnumerable<Bookshelf>> FetchBookshelves();

        Task<IEnumerable<Bookshelf>> LoadNextBookshelves();

        Task<IEnumerable<Bookshelf>> Search(string search);

        Task<bool> AddBookshelf(MvxObservableCollection<ICell> cells);

        Task<BookshelfDetail> FetchBookShelf(string id);

        Task<bool> UpdateBookShelf(ApiBookShelfRequest request);

        Task<bool> DeleteBookShelf(string id);

        #endregion
    }
}
