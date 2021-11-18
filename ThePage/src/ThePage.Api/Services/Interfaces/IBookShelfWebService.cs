using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface IBookShelfWebService
    {
        Task<ApiBookShelf> Add(ApiBookShelfRequest bookshelf);
        Task Delete(string id);
        Task<ApiBookShelfDetailResponse> GetDetail(string id);
        Task<ApiBookShelfResponse> GetList(int? page = null);
        Task<ApiBookShelfResponse> Search(string search, int? page = null);
        Task<ApiBookShelfDetailResponse> Update(string id, ApiBookShelfRequest bookshelf);
    }
}