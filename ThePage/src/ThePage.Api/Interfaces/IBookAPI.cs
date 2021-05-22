using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IBookApi
    {
        [Get("/books/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookResponse> GetBooks([Body] ApiPageRequest page);

        [Get("/books/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookDetailResponse> GetBook(string id);

        [Get("/books/search/title")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookResponse> SearchTitle([Body] ApiSearchRequest search);

        [Post("/books/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookDetailRequest> AddBook([Body] ApiBookDetailRequest book);

        [Patch("/books/v2/{Id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookDetailResponse> UpdateBook([Body] ApiBookDetailRequest book, string id);

        [Delete("/books/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteBook(string id);
    }
}
