using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IAuthorAPI
    {
        [Get("/authors/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthorResponse> GetAuthors([Body] ApiPageRequest page);

        [Get("/authors/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthor> GetAuthor(string id);

        [Post("/authors/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthor> AddAuthor([Body] ApiAuthorRequest author);

        [Patch("/authors/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthor> UpdateAuthor([Body] ApiAuthorRequest author, string id);

        [Delete("/authors/v2/{author.Id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteAuthor(ApiAuthor author);
    }
}