using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IApi { }

    public interface IAuthorApi : IApi
    {
        [Get("/authors/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthorResponse> GetAuthors([Body] ApiPageRequest page);

        [Get("/authors/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthor> GetAuthor(string id);

        [Get("/authors/search/name")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthorResponse> SearchAuthors([Body] ApiSearchRequest search);

        [Post("/authors/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthor> AddAuthor([Body] ApiAuthorRequest author);

        [Patch("/authors/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiAuthor> UpdateAuthor([Body] ApiAuthorRequest author, string id);

        [Delete("/authors/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task DeleteAuthor(string id);
    }
}