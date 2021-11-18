using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface IGoogleBooksWebService
    {
        Task<GoogleBooksResult> SearchByIsbn(string isbn);
        Task<GoogleBooksResult> SearchByTitle(string title);
    }
}