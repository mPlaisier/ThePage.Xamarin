using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IGoogleBooksApi
    {
        //https://www.googleapis.com/books/v1/volumes?q=flowers+inauthor:keyes&key=yourAPIKey
        [Get("/volumes?q={title}&key={apiKey}")]
        Task<GoogleBooksResult> SearchByTitle(string title, string apiKey);

        [Get("/volumes?q=isbn:{isbn}&key={apiKey}")]
        Task<GoogleBooksResult> SearchByIsbn(string isbn, string apiKey);
    }
}