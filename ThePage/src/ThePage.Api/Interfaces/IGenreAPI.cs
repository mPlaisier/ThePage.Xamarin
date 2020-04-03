using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IGenreAPI
    {
        [Get("/genres")]
        Task<List<Genre>> Get();

        [Get("/genres/{id}")]
        Task<Genre> Get(string id);

        [Post("/genres")]
        Task<Genre> Add([Body] Genre genre);

        [Patch("/genres/{genre.Id}")]
        Task<Genre> Update(Genre genre);

        [Delete("/genres/{genre.Id}")]
        Task Delete(Genre genre);
    }
}
