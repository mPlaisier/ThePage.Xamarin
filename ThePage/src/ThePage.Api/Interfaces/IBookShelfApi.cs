using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IBookShelfApi
    {
        [Get("/shelfs/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookShelfResponse> GetBookShelves([Body] ApiPageRequest page);

        [Get("/shelfs/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookShelfDetailResponse> GetBookShelf(string id);

        [Post("/shelfs/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookShelf> AddBookShelf([Body] ApiBookShelfRequest bookShelf);

        [Patch("/shelfs/v2/{Id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookShelfDetailResponse> UpdateBookShelf([Body] ApiBookShelfRequest bookShelf, string id);

        [Delete("/shelfs/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteBookShelf(string id);
    }
}