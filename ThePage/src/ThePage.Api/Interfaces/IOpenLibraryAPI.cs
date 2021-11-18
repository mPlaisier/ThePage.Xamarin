using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace ThePage.Api
{
    public interface IOpenLibraryApi : IApi
    {
        [Get("/books?bibkeys=ISBN:{isbn}&jscmd=data&format=json")]
        Task<Dictionary<string, OLObject>> Get(string isbn);
    }
}
