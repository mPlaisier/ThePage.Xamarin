using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface IAuthorWebService
    {
        #region Properties

        Task<ApiAuthor> Add(ApiAuthorRequest author);

        Task Delete(string id);

        Task<ApiAuthor> GetDetail(string id);

        Task<ApiAuthorResponse> GetList(int? page = null);

        Task<ApiAuthorResponse> Search(string search, int? page = null);

        Task<ApiAuthor> Update(string id, ApiAuthorRequest request);

        #endregion
    }
}