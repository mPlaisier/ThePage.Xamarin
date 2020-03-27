using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IAuthorAPI
    {
        [Get("/authors")]
        Task<List<Author>> GetAuthors();

        [Get("/authors/{id}")]
        Task<Author> GetAuthor(string id);

        [Post("/authors")]
        Task<Author> AddAuthor([Body] Author author);

        [Patch("/authors/{author.Id}")]
        Task<Author> UpdateAuthor(Author author);

        [Delete("/authors/{author.Id}")]
        Task DeleteAuthor(Author author);
    }
}