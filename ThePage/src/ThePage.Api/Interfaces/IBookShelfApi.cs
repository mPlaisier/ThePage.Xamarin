using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IBookShelfApi
    {
        [Get("/shelfs/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookShelfResponse> GetBookShelves();

        [Post("/shelfs/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookShelf> AddBookShelf([Body] ApiBookShelfRequest book);
    }
}