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

        Task<ApiBookDetailResponse> UpdateBook(ApiBookDetailResponse book);

        Task<bool> DeleteBook(ApiBookDetailResponse book);

        #endregion

        #region Author

        Task<ApiAuthorResponse> GetAllAuthors();

        Task<bool> AddAuthor(ApiAuthorRequest author);

        Task<ApiAuthor> UpdateAuthor(ApiAuthor author);

        Task<bool> DeleteAuthor(ApiAuthor author);

        #endregion

        #region Genre

        Task<ApiGenreResponse> GetAllGenres();

        Task<bool> AddGenre(ApiGenreRequest author);

        Task<ApiGenre> UpdateGenre(ApiGenre author);

        Task<bool> DeleteGenre(ApiGenre author);

        #endregion
    }
}
