using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IGenreAPI
    {
        [Get("/genres/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiGenreResponse> Get();

        [Get("/genres/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiGenre> Get(string id);

        [Post("/genres/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiGenre> Add([Body] ApiGenreRequest genre);

        [Patch("/genres/v2/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiGenre> Update([Body] ApiGenreRequest genre, string id);

        [Delete("/genres/v2/{genre.Id}")]
        [Headers("Authorization: Bearer")]
        Task Delete(ApiGenre genre);
    }
}