using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IThePageService
    {
        #region Book

        Task<ApiBookResponse> GetAllBooks();

        Task<ApiBookResponse> GetNextBooks(int page);

        Task<ApiBookDetailResponse> GetBook(string id);

        Task<ApiBookDetailRequest> AddBook(ApiBookDetailRequest book);

        Task<ApiBookDetailResponse> UpdateBook(string id, ApiBookDetailRequest book);

        Task<bool> DeleteBook(string id);

        #endregion

        #region Author

        Task<ApiAuthorResponse> GetAllAuthors();

        Task<ApiAuthorResponse> GetNextAuthors(int page);

        Task<ApiAuthor> AddAuthor(ApiAuthorRequest author);

        Task<ApiAuthor> UpdateAuthor(string id, ApiAuthorRequest author);

        Task<bool> DeleteAuthor(ApiAuthor author);

        #endregion

        #region Genre

        Task<ApiGenreResponse> GetAllGenres();

        Task<ApiGenreResponse> GetNextGenres(int page);

        Task<ApiGenre> AddGenre(ApiGenreRequest genre);

        Task<ApiGenre> UpdateGenre(string id, ApiGenreRequest genre);

        Task<bool> DeleteGenre(ApiGenre genre);

        #endregion

        #region BookShelf

        Task<ApiBookShelfResponse> GetAllBookShelves();

        Task<ApiBookShelfResponse> GetNextBookshelves(int page);

        Task<bool> AddBookShelf(ApiBookShelfRequest bookshelf);

        Task<ApiBookShelfDetailResponse> GetBookShelf(string id);

        Task<ApiBookShelfDetailResponse> UpdateBookShelf(string id, ApiBookShelfRequest bookshelf);

        Task<bool> DeleteBookShelf(string id);

        #endregion
    }
}
