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

        Task<ApiBookResponse> SearchBooksTitle(string search, int? page = null);

        Task<ApiBookDetailResponse> AddBook(ApiBookDetailRequest book);

        Task<ApiBookDetailResponse> UpdateBook(string id, ApiBookDetailRequest book);

        Task<bool> DeleteBook(string id);

        #endregion

        #region Author

        Task<ApiAuthorResponse> GetAllAuthors();

        Task<ApiAuthorResponse> GetNextAuthors(int page);

        Task<ApiAuthor> GetAuthor(string id);

        Task<ApiAuthorResponse> SearchAuthors(string search, int? page = null);

        Task<ApiAuthor> AddAuthor(ApiAuthorRequest author);

        Task<ApiAuthor> UpdateAuthor(string id, ApiAuthorRequest author);

        Task<bool> DeleteAuthor(string id);

        #endregion

        #region Genre

        Task<ApiGenreResponse> GetAllGenres();

        Task<ApiGenreResponse> GetNextGenres(int page);

        Task<ApiGenre> GetGenre(string id);

        Task<ApiGenreResponse> SearchGenres(string search, int? page = null);

        Task<ApiGenre> AddGenre(ApiGenreRequest genre);

        Task<ApiGenre> UpdateGenre(string id, ApiGenreRequest genre);

        Task<bool> DeleteGenre(string id);

        #endregion

        #region BookShelf

        Task<ApiBookShelfResponse> GetAllBookShelves();

        Task<ApiBookShelfResponse> GetNextBookshelves(int page);

        Task<ApiBookShelfResponse> SearchBookshelves(string search, int? page = null);

        Task<bool> AddBookShelf(ApiBookShelfRequest bookshelf);

        Task<ApiBookShelfDetailResponse> GetBookShelf(string id);

        Task<ApiBookShelfDetailResponse> UpdateBookShelf(string id, ApiBookShelfRequest bookshelf);

        Task<bool> DeleteBookShelf(string id);

        #endregion
    }
}
