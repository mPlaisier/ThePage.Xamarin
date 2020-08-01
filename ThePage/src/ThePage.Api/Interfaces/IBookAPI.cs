using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IBookAPI
    {
        [Get("/books/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookResponse> GetBooks();

        [Get("/books/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookDetailResponse> GetBook(string id);

        [Post("/books/v2")]
        [Headers("Authorization: Bearer")]
        Task<Book> AddBook([Body] Book book);

        [Patch("/books/v2/{book.Id}")]
        [Headers("Authorization: Bearer")]
        Task<Book> UpdateBook(Book book);

        [Delete("/books/v2/{book.Id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteBook(Book book);
    }
}
