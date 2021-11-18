using System.Threading.Tasks;

namespace ThePage.Api
{
    public interface IBaseWebService
    {
        Task<T> GetApi<T>() where T : IApi;
    }
}
