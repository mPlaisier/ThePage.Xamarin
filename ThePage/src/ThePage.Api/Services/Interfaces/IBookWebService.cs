using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface IBookWebService
    {
        Task<ApiBookDetailResponse> Add(ApiBookDetailRequest book);
        Task Delete(string id);
        Task<ApiBookDetailResponse> GetDetail(string id);
        Task<ApiBookResponse> GetList(int? page = null);
        Task<ApiBookResponse> SearchTitle(string search, int? page = null);
        Task<ApiBookDetailResponse> Update(string id, ApiBookDetailRequest book);
    }
}