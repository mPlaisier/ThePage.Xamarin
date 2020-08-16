using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IThePageService
    {
        #region Book

        Task<ApiBookResponse> GetAllBooks();

        Task<ApiBookDetailResponse> GetBook(string id);

        Task<bool> AddBook(ApiBookDetailRequest book);

        Task<ApiBookDetailResponse> UpdateBook(string id, ApiBookDetailRequest book);

        Task<bool> DeleteBook(ApiBookDetailResponse book);

        #endregion

        #region Author

        Task<ApiAuthorResponse> GetAllAuthors();

        Task<bool> AddAuthor(ApiAuthorRequest author);

        Task<ApiAuthor> UpdateAuthor(string id, ApiAuthorRequest author);

        Task<bool> DeleteAuthor(ApiAuthor author);

        #endregion

        #region Genre

        Task<ApiGenreResponse> GetAllGenres();

        Task<bool> AddGenre(ApiGenreRequest genre);

        Task<ApiGenre> UpdateGenre(string id, ApiGenreRequest genre);

        Task<bool> DeleteGenre(ApiGenre genre);

        #endregion
    }
}
