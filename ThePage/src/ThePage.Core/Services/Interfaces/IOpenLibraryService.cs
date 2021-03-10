using System.Threading.Tasks;
using ThePage.Api;

namespace ThePage.Core
{
    public interface IOpenLibraryService
    {
        Task<OLObject> GetBookByISBN(string isbn);
    }
}
