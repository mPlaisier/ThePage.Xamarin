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
        Task<ApiBookDetailResponse> AddBook([Body] ApiBookDetailRequest book);

        [Patch("/books/v2/{book.Id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookDetailResponse> UpdateBook(ApiBookDetailResponse book);

        [Delete("/books/v2/{book.Id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteBook(ApiBookDetailResponse book);
    }
}
