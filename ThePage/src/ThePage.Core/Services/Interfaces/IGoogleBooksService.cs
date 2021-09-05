using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IGoogleBooksService
    {
        Task<GoogleBooksResult> SearchBookByTitle(string title);

        Task<GoogleBooksResult> SearchBookByISBN(string isbn);
    }
}
