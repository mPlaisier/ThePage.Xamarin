using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IBookAPI
    {
        [Get("/books")]
        Task<List<Book>> GetBooks();

        [Get("/books/{id}")]
        Task<Book> GetBook(string id);

        [Post("/books")]
        Task<Book> AddBook([Body] Book book);

        [Patch("/books/{book.Id}")]
        Task<Book> UpdateBook(Book book);

        [Delete("/books/{book.Id}")]
        Task DeleteBook(Book book);

        //V2
        [Get("/books/v2")]
        [Headers("Authorization: Bearer")]
        Task<ApiBookResponse> GetBooksV2();
    }
}
