using System.Collections.Generic;
using System.Threading.Tasks;
using ThePage.Api;
using ThePage.Core.Cells;

namespace ThePage.Core
{
    public interface IBookService
    {
        string SearchText { get; }

        bool IsSearching { get; }

        Task<List<Book>> SelectBook(List<Book> books);

        Task<BookDetail> FetchBook(string id);

        Task<IEnumerable<Book>> FetchBooks();

        Task<IEnumerable<Book>> LoadNextBooks();

        Task<IEnumerable<Book>> Search(string input);

        Task<BookDetail> AddBook(IEnumerable<ICellBook> cells);

        Task<bool> DeleteBook(string id);

        Task<bool> UpdateBook(ApiBookDetailRequest request);
    }
}