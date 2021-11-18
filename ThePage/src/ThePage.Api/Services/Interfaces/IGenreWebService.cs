using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface IGenreWebService
    {
        Task<ApiGenreResponse> GetList(int? page = null);

        Task<ApiGenre> GetDetail(string id);

        Task<ApiGenreResponse> Search(string search, int? page = null);

        Task<ApiGenre> Add(ApiGenreRequest genre);

        Task<ApiGenre> Update(string id, ApiGenreRequest request);

        Task Delete(string id);
    }
}
